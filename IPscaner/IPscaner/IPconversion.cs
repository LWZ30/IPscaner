//*************************************
//IP转换
//ipTint    IP地址转换为int数字
//intTip    int数字转换为IP地址
//*************************************
class IPconversion
{
    public uint ipTint(string ipStr)
    {
        string[] ip = ipStr.Split('.');
        uint ipcode = 0xFFFFFF00 | byte.Parse(ip[3]);
        ipcode = ipcode & 0xFFFF00FF | (uint.Parse(ip[2]) << 0x8);
        ipcode = ipcode & 0xFF00FFFF | (uint.Parse(ip[1]) << 0xF);
        ipcode = ipcode & 0x00FFFFFF | (uint.Parse(ip[0]) << 0x18);
        return ipcode;
    }
    public string intTip(uint ipcode)
    {
        byte a = (byte)((ipcode & 0xFF000000) >> 0x18);
        byte b = (byte)((ipcode & 0x00FF0000) >> 0xF);
        byte c = (byte)((ipcode & 0x0000FF00) >> 0x8);
        byte d = (byte)(ipcode & 0x000000FF);
        string ipStr = string.Format("{0}.{1}.{2}.{3}", a, b, c, d);
        return ipStr;
    }
}

