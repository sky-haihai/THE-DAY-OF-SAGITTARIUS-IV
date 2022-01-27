using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XiheFramework {
    public class NotificationSvc {
        public static void PopupInfo(string context) {
            Game.Notification.Popup(context);
        }

        public static void PopupOption(string context, string leftEventName, string rightEventName) {
            Game.Notification.Confirm(context, leftEventName, rightEventName);
        }
    }
}