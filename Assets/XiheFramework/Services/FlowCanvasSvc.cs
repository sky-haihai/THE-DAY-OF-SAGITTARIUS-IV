using FlowCanvas;
using NodeCanvas.Framework;

namespace XiheFramework {
    public static class FlowCanvasSvc {
        public static void SetValue<T>(Blackboard blackboard, string key, T value) {
            blackboard.SetVariableValue(key, value);
        }

        public static T GetValue<T>(Blackboard blackboard, string key) {
            return blackboard.GetVariableValue<T>(key);
        }
    }
}