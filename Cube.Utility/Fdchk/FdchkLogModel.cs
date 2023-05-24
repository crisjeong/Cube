namespace Cube.Utility.Fdchk;

public class FdchkLogModel
{
    public FdchkLogModel()
    {
        LotId = null;
        LotType = null;
        Date = new DateTime(1999);
        Operator = null;
        Code = null;
        BinFlag = null;
        Ocr = null;
        mil = null;
        SemiInkless = null;
        Judgment = null;
        InterlockCode = null;

        waferInfs = new List<WaferInf>(25);
    }

    public string? LotId { get; set; }
    public string? LotType { get; set; }
    public DateTime Date { get; set; }
    public string? Operator { get; set; }
    public string? Code { get; set; }
    public string? BinFlag { get; set; }
    public string? Ocr { get; set; }
    public string? mil { get; set; }
    public string? SemiInkless { get; set; }
    public string? Judgment { get; set; }
    public string? InterlockCode { get; set; }
    public string? InterlockCodeDescription { get; set; }

    List<WaferInf> waferInfs { get; set; }

    public override string? ToString()
    {
        return $@"{LotId}, {LotType}, {Date.ToString()}, {Operator}, {Code}, {BinFlag}, {Ocr}, {mil}, {SemiInkless}, {Judgment}, {InterlockCode}, {InterlockCodeDescription}";
    }
}
