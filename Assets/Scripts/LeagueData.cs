using System.Collections.Generic;

[System.Serializable]
public class ClubRecord
{
    public string name;
    public string city;
    public int reputation;
    public long transferBudget;
    public ClubRecord(string n, string c, int r, long b)
    { name = n; city = c; reputation = r; transferBudget = b; }
}

public static class LeagueData
{
    // Club-name seed list. Add verified season-specific rosters and licensing separately.
    public static readonly List<ClubRecord> Ligue1Clubs = new List<ClubRecord>
    {
        new ClubRecord("Paris Saint-Germain", "Paris", 95, 80000000),
        new ClubRecord("Olympique de Marseille", "Marseille", 84, 35000000),
        new ClubRecord("AS Monaco", "Monaco", 82, 30000000),
        new ClubRecord("Olympique Lyonnais", "Lyon", 80, 22000000),
        new ClubRecord("LOSC Lille", "Lille", 79, 25000000),
        new ClubRecord("OGC Nice", "Nice", 76, 18000000),
        new ClubRecord("Stade Rennais", "Rennes", 76, 20000000),
        new ClubRecord("RC Lens", "Lens", 75, 16000000),
        new ClubRecord("RC Strasbourg Alsace", "Strasbourg", 72, 14000000),
        new ClubRecord("Toulouse FC", "Toulouse", 69, 9000000),
        new ClubRecord("FC Nantes", "Nantes", 68, 8000000),
        new ClubRecord("Montpellier HSC", "Montpellier", 67, 7000000),
        new ClubRecord("Stade de Reims", "Reims", 67, 7000000),
        new ClubRecord("Le Havre AC", "Le Havre", 65, 6000000),
        new ClubRecord("AJ Auxerre", "Auxerre", 64, 5000000),
        new ClubRecord("Angers SCO", "Angers", 63, 4500000),
        new ClubRecord("AS Saint-Étienne", "Saint-Étienne", 66, 6500000),
        new ClubRecord("FC Lorient", "Lorient", 64, 5000000)
    };
}
