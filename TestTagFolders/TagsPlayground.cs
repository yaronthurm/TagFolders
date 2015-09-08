using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;
using YaronThurm.TagFolders;

namespace TestTagFolders
{
    public static class TagsPlayground
    {
        public static void Play()
        {
            var e1 = new FilesWereTagged { FileNames = new[] { "file1", "file2" }, TagNames = new[] { "tag1", "tag2" } };
            var e2 = new FilesWereTagged { FileNames = new[] { "file1", "file2" }, TagNames = new[] { "tag11", "tag22" } };
            var e3 = new FilesWereTagged { FileNames = new[] { "file1", "file3" }, TagNames = new[] { "tag3", "tag4" } };
            var e4 = new FilesWereTagged { FileNames = new[] { "file4", "file5" }, TagNames = new[] { "tag1", "tag4" } };
            var e5 = new TagRenamed { OldValue = "tag1", NewValue = "tag10" };
            var e6 = new FilesWereUnTagged { FileNames = new[] { "file1", "file2" }, TagNames = new[] { "tag2" } };
            var e7 = new FileRenamed { OldValue = "file1", NewValue = "file10" };

            Event[] events = { e1, e2, e3, e4, e5, e6 };
            foreach (var ev in events)
            {
                var processor = ev as IEventProcessor;
                processor.ProcessEvent();
            }

            var state = State.Populate(FilesWithTagsRepository.GetFilesList());


            var filter = new TagsIntersectionCondition(
                new TagsUnionCondition(InversableTag.GetTag("tag10"), InversableTag.GetTag("tag2")),
                new TagsUnionCondition(InversableTag.GetTag("tag11"), InversableTag.GetTag("tag4"))
                );

            var newState = State.Populate(filter.Apply(state.GetFiles()));

            filter = new TagsIntersectionCondition(
                new TagsUnionCondition(InversableTag.GetInverseTag("tag10"))
                );

            newState = State.Populate(filter.Apply(state.GetFiles()));
        }
    }


    public class State
    {
        private Dictionary<Tag, int /*count*/> _tags = new Dictionary<Tag, int>();
        private List<FileWithTags> _files = new List<FileWithTags>();


        public static State Populate(List<FileWithTags> files)
        {
            var ret = new State();
            ret._files.AddRange(files);

            var allTags = files.SelectMany(x => x.Tags);
            foreach (var tag in allTags)
            {
                if (ret._tags.ContainsKey(tag))
                {
                    ret._tags[tag]++;
                }
                else
                {
                    ret._tags[tag] = 1;
                }
            }

            return ret;
        }


        public List<FileWithTags> GetFiles()
        {
            return _files;
        }
    }


    #region Intersection and Union
    public class TagsIntersectionCondition
    {
        public List<TagsUnionCondition> Items;

        public TagsIntersectionCondition(params TagsUnionCondition[] items)
        {
            this.Items = items.ToList();
        }

        public List<FileWithTags> Apply(List<FileWithTags> source)
        {
            var ret = source.Where(x => this.IsMatchAll(x.Tags)).ToList();
            return ret;
        }


        private bool IsMatchAll(List<Tag> source)
        {
            foreach (TagsUnionCondition unionCondition in this.Items)
            {
                bool isMatch = unionCondition.IsMatch(source);
                if (!isMatch)
                    return false;
            }
            return true;
        }
    }

    public class TagsUnionCondition{
        public List<InversableTag> Items;

        public TagsUnionCondition(params InversableTag[] items)
        {
            this.Items = items.ToList();
        }

        public bool IsMatch(List<Tag> source){    
            foreach (InversableTag itag in this.Items){
                bool sourceContainsCurrent = source.Contains(itag.Tag);
                if (itag.Inverse && !sourceContainsCurrent) 
                    return true;
                if (!itag.Inverse && sourceContainsCurrent)
                    return true;
            }
            return false;       
        }
    }
    #endregion


    #region Events
    public abstract class Event
    {
        //public DateTime Time = DateTime.UtcNow;
    }

    public class FilesWereTagged : Event, IEventProcessor
    {
        public string[] FileNames;
        public string[] TagNames;

        public void ProcessEvent()
        {            
            foreach (string fileName in this.FileNames)
            {
                var file = FilesWithTagsRepository.GetOrCreate(fileName);
                foreach (string tagName in this.TagNames)
                {
                    var tag = TagsRepository.GetOrCreate(tagName);
                    file.Tags.Add(tag);
                }
            }
        }
    }

    public class FilesWereUnTagged : Event, IEventProcessor
    {
        public string[] FileNames;
        public string[] TagNames;

        public void ProcessEvent()
        {
            foreach (string fileName in this.FileNames)
            {
                var file = FilesWithTagsRepository.GetOrCreate(fileName);
                foreach (string tagName in this.TagNames)
                {
                    var tag = TagsRepository.GetOrCreate(tagName);
                    file.Tags.Remove(tag);
                }
            }
        }
    }

    public class TagRenamed : Event, IEventProcessor
    {
        public string OldValue;
        public string NewValue;

        public void ProcessEvent()
        {
            TagsRepository.Rename(OldValue, NewValue);
        }
    }

    public class FileRenamed : Event, IEventProcessor
    {
        public string OldValue;
        public string NewValue;

        public void ProcessEvent()
        {
            FilesWithTagsRepository.Rename(OldValue, NewValue);
        }
    }

    public interface IEventProcessor
    {
        void ProcessEvent();
    }
    #endregion

    
    #region Tags and files
    public class Tag
    {
        public string Value;

        public override string ToString()
        {
            return Value;
        }
    }

    public class InversableTag
    {
        public Tag Tag;
        public bool Inverse;


        public static InversableTag GetTag(string value){
            return new InversableTag{ Tag= TagsRepository.GetOrCreate(value), Inverse = false};
        }

        public static InversableTag GetInverseTag(string value){
            return new InversableTag{ Tag= TagsRepository.GetOrCreate(value), Inverse = true};
        }

        public override string ToString()
        {
            if (this.Inverse)
                return this.Tag.Value + "'";
            else
                return this.Tag.Value;
        }
    }



    public class FileWithTags
    {
        public string FileName;
        public List<Tag> Tags;

        public override string ToString()
        {
            return "File: " + FileName + " (Tags: " + string.Join(",", Tags.Select(x => x.Value)) + ")";
        }
    }
    #endregion


    #region Repositories
    public class TagsRepository
    {
        private static ConcurrentDictionary<string, Tag> _tags = new ConcurrentDictionary<string,Tag>();

        public static Tag GetOrCreate(string tagValue)
        {
            var ret = _tags.GetOrAdd(tagValue, x => new Tag { Value = x });
            return ret;
        }

        public static void Rename(string oldValue, string newValue)
        {
            Tag tag;
            if (_tags.TryRemove(oldValue, out tag))
            {
                tag.Value = newValue;
                _tags[newValue] = tag;
            }
            else
            {
                throw new ApplicationException(string.Format("tag '{0}' does not exist", oldValue));
            }
        }
    }

    public class FilesWithTagsRepository
    {
        private static ConcurrentDictionary<string, FileWithTags> _filesWithTags = new ConcurrentDictionary<string,FileWithTags>();

        public static FileWithTags GetOrCreate(string filename)
        {
            var ret = _filesWithTags.GetOrAdd(filename, x => new FileWithTags { FileName = x, Tags = new List<Tag>() });
            return ret;
        }

        public static void Rename(string oldValue, string newValue)
        {
            FileWithTags tag;
            if (_filesWithTags.TryRemove(oldValue, out tag))
            {
                tag.FileName = newValue;
                _filesWithTags[newValue] = tag;
            }
            else
            {
                throw new ApplicationException(string.Format("file '{0}' does not exist", oldValue));
            }
        }


        public static List<FileWithTags> GetFilesList()
        {
            return _filesWithTags.Values.ToList();
        }
    }
    #endregion
}
