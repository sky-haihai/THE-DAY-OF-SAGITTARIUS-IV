namespace XiheFramework {
    /// <summary>
    /// shortcut to get all component
    /// </summary>
    public static class Game {
        public static AudioModule Audio => GameManager.GetModule<AudioModule>();

        public static EventModule Event => GameManager.GetModule<EventModule>();

        public static DebugModule Log => GameManager.GetModule<DebugModule>();

        public static InputModule Input => GameManager.GetModule<InputModule>();

        // public static SceneModule Scene => GameManager.GetModule<SceneModule>();

        public static BlackboardModule Blackboard => GameManager.GetModule<BlackboardModule>();

        public static SerializationModule Serialization => GameManager.GetModule<SerializationModule>();

        public static UIModule UI => GameManager.GetModule<UIModule>();

        public static LocalizationModule Localization => GameManager.GetModule<LocalizationModule>();

        public static NotificationModule Notification => GameManager.GetModule<NotificationModule>();

        public static StateMachineModule Fsm => GameManager.GetModule<StateMachineModule>();

        public static CameraModule Camera => GameManager.GetModule<CameraModule>();
    }
}