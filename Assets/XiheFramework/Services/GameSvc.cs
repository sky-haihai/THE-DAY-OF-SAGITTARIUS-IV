namespace XiheFramework {
    public static class GameSvc {
        public static void ShutDown(ShutDownType shutDownType) {
            GameManager.ShutDown(shutDownType);
        }
    }
}