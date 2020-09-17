﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Base64Util  {

    ///编码
    public static string EncodeBase64(string code_type, string code)
    {
        string encode = "";
        byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
        try
        {
            encode = Convert.ToBase64String(bytes);
        }
        catch
        {
            encode = code;
        }
        return encode;
    }
    ///解码
    public static string DecodeBase64(string code_type, string code)
    {
        string decode = "";
        byte[] bytes = Convert.FromBase64String(code);
        try
        {
            decode = Encoding.GetEncoding(code_type).GetString(bytes);
        }
        catch
        {
            decode = code;
        }
        return decode;
    }
}