using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Unity.PlasticSCM.Editor.UI
{
    internal class NotificationDrawer
    {
        private struct NotifyMessage
        {
            internal string Message { get; private set; }
            internal MessageType MessageType { get; private set; }
            internal Images.Name ImageName { get; private set; }

            internal NotifyMessage(string message, MessageType messageType, Images.Name imageName) :this()
            {
                this.Message = message;
                this.MessageType = messageType;
                this.ImageName = imageName;
            }
        }

        public NotificationDrawer()
        {
            notifications = new Queue<NotifyMessage>();
        }

        internal void Notify(string message, MessageType type, Images.Name imageName)
        {
            NotifyMessage notifyMessage = new NotifyMessage(message, type, imageName);
            notifications.Enqueue(notifyMessage);
            Task.Delay(AUTO_HIDE_DELAY).ContinueWith(t => notifications.Dequeue());
        }

        internal void DoDrawer()
        {
            foreach (NotifyMessage notifyMessage in notifications)
            {
                if (notifyMessage.MessageType!= MessageType.None)
                {
                    EditorGUILayout.HelpBox(notifyMessage.Message, notifyMessage.MessageType);
                }
                else
                {
                    if (notifyMessage.ImageName != Images.Name.None)
                    {
                        GUIStyle style = new GUIStyle(EditorStyles.helpBox);
                        style.richText = true;
                        style.fontSize=14;

                        GUIContent stepLabelContent = new GUIContent(
                            string.Format(" {0}", notifyMessage.Message),
                            Images.GetImage(notifyMessage.ImageName));

                        GUILayout.Label(
                           stepLabelContent,
                           style,
                           GUILayout.Height(40));
                    }
                }
            }
        }

        private Queue<NotifyMessage> notifications;
        const int AUTO_HIDE_DELAY = 5000;
    }
}