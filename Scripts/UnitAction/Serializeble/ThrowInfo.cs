using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [Serializable]
    public class ThrowInfo
    {
        public string ThrowActionData; // ���[�V��������Frame��񂾂��Q�Ƃ���
        public int ThrowID; // �����ZID
        public int DownID; // �_�E��ID
        public string frames; // A1,A3,C11,C22 �Ƃ����Ɗy ,��؂�Ŏ擾�Ƃ�
    }
}