using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenNashCalculator
{
    interface HandHistoryReader
    {
        /// <summary>指定したファイルの指定したハンド数前のハンドヒストリーを読み込む</summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="backNum">何ハンド前か？</param>
        /// <returns>読み込んだ結果のテーブル状況(NashCalculateするために用いる)</returns>
        TableData read(string fileName, int backNum);

        string getTourneyID(string fileName);
        DateTime GetLastWriteTime(string fileName);
    }

    enum Blind
    {
        small, big
    }
}
