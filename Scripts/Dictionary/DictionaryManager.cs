using Kogane;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    // String, String
    [Serializable]
    public class StringKeyStringValuePair : SerializableKeyValuePair<string, string> { }
    [Serializable]
    public class DictionaryStringString : SerializableDictionary<string, string, StringKeyStringValuePair> { }
}

