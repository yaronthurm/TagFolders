using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TestTagFolders
{
    public static class TagsPlayground
    {
        public static void Play()
        {
            var e1 = new TagsWereAddedToFiles { FileNames = new[] { "file1", "file2" }, TagsThatWereAdded = new[] { "tag1", "tag2" } };
            var e2 = new TagsWereAddedToFiles { FileNames = new[] { "file1", "file2" }, TagsThatWereAdded = new[] { "tag11", "tag22" } };
            var e3 = new TagsWereAddedToFiles { FileNames = new[] { "file1", "file3" }, TagsThatWereAdded = new[] { "tag3", "tag4" } };
            var e4 = new TagsWereAddedToFiles { FileNames = new[] { "file4", "file5" }, TagsThatWereAdded = new[] { "tag1", "tag4" } };
            var e5 = new TagRenamed { OldValue = "tag1", NewValue = "tag10" };
            var e6 = new TagsWereRemovedFromFiles { FileNames = new[] { "file1", "file2" }, TagsThatWereRemoved = new[] { "tag2" } };
            var e7 = new FileRenamed { OldValue = "file1", NewValue = "file10" };

            Event[] events = { e1, e2, e3, e4, e5, e6 };
            foreach (var ev in events)
            {
                var processor = ev as IEventProcessor;
                processor.ProcessEvent();
            }

            var state = State.Populate(TaggedFile.Repository.GetFilesList());


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
        private static JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
            Formatting = Newtonsoft.Json.Formatting.None,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.ReadAhead
        };


        private Dictionary<Tag, int /*count*/> _tags = new Dictionary<Tag, int>();
        private List<TaggedFile> _files = new List<TaggedFile>();

        private static string _filename = "D:\\tags.txt";


        public static void LoadFromFile()
        {
            if (File.Exists(_filename))
            {
                var events = File.ReadAllLines(_filename)
                    .Select(x => Newtonsoft.Json.JsonConvert.DeserializeObject<Event>(x, settings));
                foreach (var ev in events)
                {
                    var processor = ev as IEventProcessor;
                    processor.ProcessEvent();
                }
            }
        }

        public static void AppendEventToFile(Event ev)
        {
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(ev, settings);
            File.AppendAllLines(_filename, new[] { text });
        }

        public static State Populate(List<TaggedFile> files)
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


        public List<TaggedFile> GetFiles()
        {
            return _files;
        }

        public Dictionary<Tag, int /*count*/> GetTags()
        {
            return _tags;
        }

        internal static void AddAndSaveEvent(Event ev)
        {
            var processor = ev as IEventProcessor;
            processor.ProcessEvent();
            State.AppendEventToFile(ev);
        }
    }


    #region Intersection and Union
    public class TagsIntersectionCondition
    {
        private List<TagsUnionCondition> Items;

        public TagsIntersectionCondition()
        {
            this.Items = new List<TagsUnionCondition>();
        }

        public TagsIntersectionCondition(params TagsUnionCondition[] items)
        {
            this.Items = items.ToList();
        }

        public void AddUnionCondition(TagsUnionCondition condition)
        {
            this.Items.Add(condition);
        }

        public IEnumerable<TagsUnionCondition> UnionConditions
        {
            get { return this.Items.Where(x => x.Count > 0); }
        }

        public List<TaggedFile> Apply(IEnumerable<TaggedFile> source)
        {
            if (this.Items.Count == 0)
                return source.ToList();

            var ret = source.Where(x => this.IsMatchAll(x.Tags)).ToList();
            return ret;
        }
        
        public List<Tag> AllTags()
        {
            var ret = new List<Tag>();
            foreach (var item in this.Items)
            {
                ret.AddRange(item.AllTags());
            }
            return ret;
        }

        private bool IsMatchAll(IEnumerable<Tag> source)
        {
            foreach (TagsUnionCondition unionCondition in this.Items)
            {
                bool isMatch = unionCondition.IsMatch(source);
                if (!isMatch)
                    return false;
            }
            return true;
        }

        internal void Remove(InversableTag inversableTag)
        {
            foreach (var item in this.Items)
                item.Remove(inversableTag);
        }
    }

    public class TagsUnionCondition{
        private List<InversableTag> Items;

        public TagsUnionCondition(params InversableTag[] items)
        {
            this.Items = items.ToList();
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public IEnumerable<InversableTag> InversableTags
        {
            get { return this.Items; }
        }



        public List<Tag> AllTags()
        {
            var ret = this.Items.Select(x => x.Tag).ToList();
            return ret;
        }

        public bool IsMatch(IEnumerable<Tag> source){
            if (this.Items.Count == 0)
                return true;

            foreach (InversableTag itag in this.Items){
                bool sourceContainsCurrent = source.Contains(itag.Tag);
                if (itag.Inverse && !sourceContainsCurrent) 
                    return true;
                if (!itag.Inverse && sourceContainsCurrent)
                    return true;
            }
            return false;       
        }

        internal void Remove(InversableTag inversableTag)
        {
            this.Items.Remove(inversableTag);
        }
    }
    #endregion


    #region Events
    public abstract class Event
    {
        //public DateTime Time = DateTime.UtcNow;
    }

    public class TagsWereAddedToFiles : Event, IEventProcessor
    {
        public string[] FileNames;
        public string[] TagsThatWereAdded;

        public void ProcessEvent()
        {            
            foreach (string fileName in this.FileNames)
            {
                var file = TaggedFile.Repository.GetOrCreate(fileName);
                foreach (string tagName in this.TagsThatWereAdded)
                {
                    var tag = Tag.Repository.GetOrCreate(tagName);
                    file.Tags.Add(tag);
                }
            }
        }
    }

    public class TagsWereRemovedFromFiles : Event, IEventProcessor
    {
        public string[] FileNames;
        public string[] TagsThatWereRemoved;

        public void ProcessEvent()
        {
            foreach (string fileName in this.FileNames)
            {
                var file = TaggedFile.Repository.GetOrCreate(fileName);
                foreach (string tagName in this.TagsThatWereRemoved)
                {
                    var tag = Tag.Repository.GetOrCreate(tagName);
                    file.Tags.Remove(tag);
                }
            }
        }
    }

    public class TagsWereUpdatedForSomeFiles : Event, IEventProcessor
    {
        public string[] FileNames;
        public string[] TagNames;

        public void ProcessEvent()
        {
            foreach (string fileName in this.FileNames)
            {
                var file = TaggedFile.Repository.GetOrCreate(fileName);
                file.Tags.Clear();
                foreach (string tagName in this.TagNames)
                {
                    var tag = Tag.Repository.GetOrCreate(tagName);
                    file.Tags.Add(tag);
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
            Tag.Repository.Rename(OldValue, NewValue);
        }
    }

    public class FileRenamed : Event, IEventProcessor
    {
        public string OldValue;
        public string NewValue;

        public void ProcessEvent()
        {
            TaggedFile.Repository.Rename(OldValue, NewValue);
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
        public string Value {get; private set;}

        private Tag(string value)
        {
            this.Value = value;
        }


        public override string ToString()
        {
            return Value;
        }


        public class Repository
        {
            private static ConcurrentDictionary<string, Tag> _tags = new ConcurrentDictionary<string, Tag>();

            public static Tag GetOrCreate(string tagValue)
            {
                var ret = _tags.GetOrAdd(tagValue, x => new Tag(x));
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
    }

    public class InversableTag
    {
        public Tag Tag;
        public bool Inverse;


        public static InversableTag GetTag(string value){
            return new InversableTag{ Tag= Tag.Repository.GetOrCreate(value), Inverse = false};
        }

        public static InversableTag GetInverseTag(string value){
            return new InversableTag{ Tag= Tag.Repository.GetOrCreate(value), Inverse = true};
        }

        public override string ToString()
        {
            if (this.Inverse)
                return this.Tag.Value + "'";
            else
                return this.Tag.Value;
        }
    }



    public class TaggedFile
    {        
        public List<Tag> Tags { get; private set; }
        public string FileName {get; private set;}
        

        private TaggedFile(string filename)
        {
            this.Tags = new List<Tag>();
            this.FileName = filename;
        }

        public override string ToString()
        {
            return "File: " + FileName + " (Tags: " + string.Join(",", Tags.Select(x => x.Value)) + ")";
        }

        public class Repository
        {
            private static ConcurrentDictionary<string, TaggedFile> _filesWithTags = new ConcurrentDictionary<string, TaggedFile>();

            public static TaggedFile GetOrCreate(string filename)
            {
                var ret = _filesWithTags.GetOrAdd(filename, x => new TaggedFile(x));
                return ret;
            }

            public static void Rename(string oldValue, string newValue)
            {
                TaggedFile tag;
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


            public static List<TaggedFile> GetFilesList()
            {
                return _filesWithTags.Values.ToList();
            }
        }
    }

    #endregion
}
