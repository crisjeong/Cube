using System.Text;

namespace Cube.Utility.Fdchk;

public class FdchkLogParser
{
    FdchkLogModel _fdchkLogModel;

    public FdchkLogParser()
    {
        _fdchkLogModel = new FdchkLogModel();
    }

    public (bool, string) Deserialize(string fileName)
    {
        if (fileName is null)
            throw new ArgumentNullException(nameof(fileName));

        ErrorType ErrorType = ErrorType.NoError;
        var lines = ReadLines(streamProvider: () => new FileStream(fileName, FileMode.Open), Encoding.UTF8)
                            .ToList();

        int lineCount = 0;
        foreach (var line in lines)
        {
            lineCount++;

            if (IsHeaderContents(lineCount))
            {
                var parts = line.Split(new char[] { '\n', '=' }, StringSplitOptions.RemoveEmptyEntries);

                //Key 값은 있어야 파싱한다. Key 값도 없으면 format 에러
                if (parts.Length < 1)
                {
                    //invalid format
                    //Header Contents 중 어떤 항목이 없는지 구별하기 위해 ...
                    ErrorType = (ErrorType.LotIdNotFound + lineCount - 1);
                    goto _exit;
                }

                (bool isSuccess, ErrorType ErrorType) result = DeserializeHeaderLineByLine(parts[0].Trim(),
                                                                                              ((parts.Length > 1) ? parts[1].Trim() : string.Empty));
                if (!result.isSuccess)
                {
                    Console.WriteLine($"FDCHK log deserialize failed : line= {lineCount}, error= {result.ErrorType.ToString()}");
                    ErrorType = result.ErrorType;
                    goto _exit;
                }
            }
            else
            {
                //TODO - body contents parsing
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                //Key 값은 있어야 파싱한다. Key 값도 없으면 format 에러
                if (parts.Length < 1)
                {
                    //invalid format
                    //Header Contents 중 어떤 항목이 없는지 구별하기 위해 ...
                    ErrorType = (ErrorType.WaferInfoNotFound);
                    goto _exit;
                }

                (bool isSuccess, ErrorType ErrorType) result = DeserializeBodyLineByLine(parts);
                if (!result.isSuccess)
                {
                    Console.WriteLine($"FDCHK log deserialize failed : line= {lineCount}, error= {result.ErrorType.ToString()}");
                    ErrorType = result.ErrorType;
                    goto _exit;
                }

            }
        }

        //한번 더 체크
        return DeserializedDataIsValid();

    _exit:
        return ((ErrorType == ErrorType.NoError), ErrorType.ToString());
    }

    private IEnumerable<string> ReadLines(Func<Stream> streamProvider, Encoding encoding)
    {
        using (var stream = streamProvider())
        using (var reader = new StreamReader(stream, encoding))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }

    private bool IsHeaderContents(int lineCount /*1' based*/)
    {
        //Header Contents 갯수가 10개로 고정
        return (lineCount > 0 && lineCount <= 10);
    }

    public (bool, ErrorType) DeserializeHeaderLineByLine(string key, string value)
    {
        ErrorType ErrorType = ErrorType.NoError;

        switch (key)
        {
            case "LOT": _fdchkLogModel.LotId = value; break;
            case "TYPE": _fdchkLogModel.LotType = value; break;
            case "DATE": _fdchkLogModel.Date = DateTime.Parse(value); break;
            case "OPERATOR": _fdchkLogModel.Operator = value; break;
            case "CODE": _fdchkLogModel.Code = value; break;
            case "BIN_FLAG": _fdchkLogModel.BinFlag = value; break;
            case "OCR": _fdchkLogModel.Ocr = value; break;
            case "1MIL": _fdchkLogModel.mil = value; break;
            case "SEMI_INKLESS": _fdchkLogModel.SemiInkless = value; break;

            //ERR=0 < 000 : PASS >
            //PASS is ERR=0
            //FAIL is ERR=1
            case "ERR":

                string[] parts = value.Split(new char[] { '<', '>', ':', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3)
                {
                    //split 된 tokens 가 3개여야 유효함
                    ErrorType = ErrorType.InvalidFormat;
                }
                else
                {
                    _fdchkLogModel.Judgment = parts[0].Trim();
                    _fdchkLogModel.InterlockCode = parts[1].Trim();
                    _fdchkLogModel.InterlockCodeDescription = parts[2].Trim();
                }

                break;

            /* 정의되지 않은 헤더 정보 */
            default:
                ErrorType = ErrorType.InvalidFormat;
                break;
        }

        return ((ErrorType == ErrorType.NoError), ErrorType);
    }

    public (bool, ErrorType) DeserializeBodyLineByLine(string[] body)
    {
        ErrorType ErrorType = ErrorType.NoError;
        var key = body[0];

        switch (key)
        {
            case "W":
                break;

            case "G":

                if (body.Length < 2)
                {
                    //split 된 pars 가 2개여야 유효함
                    ErrorType = ErrorType.InvalidFormat;
                }
                else
                {

                }

                break;

            /* 정의되지 않은 헤더 정보 */
            default:
                ErrorType = ErrorType.InvalidFormat;
                break;
        }

        return ((ErrorType == ErrorType.NoError), ErrorType);
    }

    private (bool, string) DeserializedDataIsValid()
    {
        if (_fdchkLogModel.LotId is null)
            return (false, ErrorType.LotIdNotFound.ToString());

        if (_fdchkLogModel.LotType is null)
            return (false, ErrorType.LotTypeNotFound.ToString());

        if (_fdchkLogModel.BinFlag is null)
            return (false, ErrorType.BinFlagNotFound.ToString());

        if (_fdchkLogModel.Operator is null)
            return (false, ErrorType.OperatorNotFound.ToString());

        if (_fdchkLogModel.Code is null)
            return (false, ErrorType.CodeNotFound.ToString());

        if (_fdchkLogModel.Ocr is null)
            return (false, ErrorType.OcrNotFound.ToString());

        if (_fdchkLogModel.mil is null)
            return (false, ErrorType.MilNotFound.ToString());

        if (_fdchkLogModel.SemiInkless is null)
            return (false, ErrorType.SemiInklessNotFound.ToString());

        if (_fdchkLogModel.Judgment is null)
            return (false, ErrorType.JudgmentNotFound.ToString());

        if (_fdchkLogModel.InterlockCode is null)
            return (false, ErrorType.InterlockCodeNotFound.ToString());

        if (_fdchkLogModel.InterlockCodeDescription is null)
            return (false, ErrorType.InterlockCodeDescriptionNotFound.ToString());

        return (true, ErrorType.NoError.ToString());
    }
}
