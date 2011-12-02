using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTRtoNote2.Model
{
    public interface IExporter
    {
        /// <summary>
        /// データを出力する
        /// </summary>
        /// <param name="fileName">出力ファイル名</param>
        /// <param name="filePath">出力ファイルパス</param>
        void Export(string filePath);
    }
}
