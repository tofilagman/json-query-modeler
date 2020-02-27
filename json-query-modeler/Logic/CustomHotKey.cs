using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace json_query_modeler.Logic
{
    [Serializable]
    public class CustomHotKey : HotKey
    {
        private readonly EventHandler Handler;

        public CustomHotKey(string name, Key key, ModifierKeys modifiers, bool enabled, EventHandler @event)
            : base(key, modifiers, enabled)
        {
            Name = name;
            Handler = @event;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged(name);
                }
            }
        }

        protected override void OnHotKeyPress()
        {
            //MessageBox.Show(string.Format("'{0}' has been pressed ({1})", Name, this)); 
            Handler.Invoke(this, new HotKeyEventArgs(this));
            base.OnHotKeyPress();
        }


        protected CustomHotKey(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            Name = info.GetString("Name");
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Name", Name);
        }
    }
}
