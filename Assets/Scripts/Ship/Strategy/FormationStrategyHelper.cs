public static class FormationStrategyHelper {
    public static IFormationStrategy GetStrategyByFormation(Formations formation) {
        return formation switch {
            Formations.Spread => new SpreadFormation(),
            Formations.Line => new LineFormation(),
            Formations.Wedge => new WedgeFormation(),
            Formations.Circular => new CircularFormation(),
            Formations.BentLine => new BentLineFormation(),
            Formations.Horseshoe => new HorseshoeFormation(),
            _ => new SpreadFormation()
        };
    }
}