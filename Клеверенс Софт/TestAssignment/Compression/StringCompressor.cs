namespace TestAssignment.Compression;

public static class StringCompressor
{
    public static string Compress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder();
        int i = 0;

        while (i < input.Length)
        {
            char current = input[i];
            int count = 1;

            while (i + count < input.Length && input[i + count] == current)
                count++;

            result.Append(current);
            if (count > 1)
                result.Append(count);

            i += count;
        }

        return result.ToString();
    }

    public static string Decompress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder();
        int i = 0;

        while (i < input.Length)
        {
            char current = input[i];
            i++;

            string countStr = "";
            while (i < input.Length && char.IsDigit(input[i]))
            {
                countStr += input[i];
                i++;
            }

            int count = countStr.Length > 0 ? int.Parse(countStr) : 1;
            result.Append(current, count);
        }

        return result.ToString();
    }
}
