namespace Cube.Utility.Fdchk;

public enum ErrorType
{
    NoError = 0,
    InvalidFormat = 1,
    KeyNotFound = 2,
    FileAccessDenied = 3,
    WaferInfoNotFound = 4,

    UnknownError = 9,


    // add more error codes as needed
    LotIdNotFound = 10,
    LotTypeNotFound = 11,
    OperatorNotFound = 12,
    CodeNotFound = 13,
    BinFlagNotFound = 14,
    OcrNotFound = 15,
    MilNotFound = 16,
    SemiInklessNotFound = 17,
    JudgmentNotFound = 18,
    InterlockCodeNotFound = 19,
    InterlockCodeDescriptionNotFound = 20
}
