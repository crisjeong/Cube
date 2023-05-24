namespace Cube.Utility.Fdchk;

/// <summary>
/// WaferNo : Wafer No 값으로 2자리 숫자로 표현된 문자열 01, 02 ~ 25
/// Exist : 해당 랏이 포함된 Wafer 인지 
/// InterlockCode : FDCHK 를 통한 Wafer 별 판정 결과 
/// </summary>
public class WaferInf
{
    public string? WaferNo { get; set; }
    public bool Exist { get; set; }
    public string? InterlockCode { get; set; }
    public int GoodChipCount { get; set; }
}