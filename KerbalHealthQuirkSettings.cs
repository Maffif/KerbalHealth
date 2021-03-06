﻿namespace KerbalHealth
{
    class KerbalHealthQuirkSettings : GameParameters.CustomParameterNode
    {
    public override string Title => "Quirks";
    public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;
    public override bool HasPresets => true;
    public override string Section => "Kerbal Health (2)";
    public override string DisplaySection => Section;
    public override int SectionOrder => 2;

        [GameParameters.CustomParameterUI("Quirks Enabled", toolTip = "Quirks can be awarded to kerbals and affect their health stats")]
        public bool QuirksEnabled = true;

        [GameParameters.CustomIntParameterUI("Max Quirks", toolTip = "Maximum number of quirks for a kerbal", minValue = 0, maxValue = 5, displayFormat = "N0", stepSize = 1)]
        public int MaxQuirks = 2;

        [GameParameters.CustomFloatParameterUI("Chance of Getting a Quirk", toolTip = "Chance of a kerbal being awarded a quirk when he/she levels up", minValue = 0, maxValue = 1, displayFormat = "N2", asPercentage = true, stepCount = 21)]
        public float QuirkChance = 0.2f;

        [GameParameters.CustomParameterUI("Kerbal Stats Affect Quirk Weights", toolTip = "Chances of getting some quirks depend on Courage and Stupidity of the kerbal")]
        public bool StatsAffectQuirkWeights = true;
    }
}
