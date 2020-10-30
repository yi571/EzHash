using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace EzHash {
    class Program {
        static void Main(string[] args) {
            //string type = "";
            var typeOperate = Type.比對Hash;
            int[] types = new int[] { 1,2,3};
            while (typeOperate != Type.close) {
                Console.WriteLine("請選擇模式：(1.比對Hash 2.產生Hash 3.close)");
                //type = 
                string tmpEnum = Console.ReadLine();
                if (int.TryParse(tmpEnum, out int intEnum) && types.Any(i => i == intEnum)) {
                    typeOperate = (Type)intEnum;
                    if (typeOperate == Type.close) {
                        return;
                    }
                    Console.WriteLine("請輸入檔案路徑：");
                    string filePath = Console.ReadLine();
                    

                    try {
                        FileStream fileStream = new FileStream(filePath, FileMode.Open);
                        fileStream.Position = 0;

                        Dictionary<string, string> valuePairs = new Dictionary<string, string>();

                        using MD5 fileMd5 = MD5.Create();
                        byte[] md5Value = fileMd5.ComputeHash(fileStream);
                        valuePairs.Add("MD5", PrintByteArray(md5Value));

                        using SHA1 filesHA1 = SHA1.Create();
                        byte[] sha1Value = filesHA1.ComputeHash(fileStream);
                        valuePairs.Add("SHA1", PrintByteArray(sha1Value));

                        using SHA256 fileSha256 = SHA256.Create();
                        byte[] sha256Value = fileSha256.ComputeHash(fileStream);
                        valuePairs.Add("SHA256", PrintByteArray(sha256Value));

                        switch (typeOperate) {
                            case Type.比對Hash:
                                Console.WriteLine("請輸入校驗碼：");
                                string inputHash = Console.ReadLine().ToLower();
                                if (valuePairs.Any(i => i.Value == inputHash)) {
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.WriteLine("校驗碼符合");
                                    Console.ResetColor();
                                }
                                else {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine("校驗碼不符合");
                                    Console.ResetColor();
                                }
                                break;
                            case Type.產生Hash:
                                foreach (var item in valuePairs) {
                                    Console.WriteLine($"{item.Key}:");
                                    Console.WriteLine(item.Value);
                                    Console.WriteLine();
                                }
                                break;
                            case Type.close:
                                break;
                            default:
                                break;
                        }
                        fileStream.Close();


                    }
                    catch (IOException e) {
                        Console.WriteLine($"I/O Exception: {e.Message}");
                    }
                    catch (UnauthorizedAccessException e) {
                        Console.WriteLine($"Access Exception: {e.Message}");
                    }
                }
                else {
                    Console.WriteLine("模式錯誤！");
                }
                Console.WriteLine("=======");

            }


        }

        public static string PrintByteArray(byte[] array) {
            string printStr = "";
            for (int i = 0; i < array.Length; i++) {
                printStr += $"{array[i]:X2}";
                
            }
            return printStr.ToLower();
        }

        enum Type {
            比對Hash=1,
            產生Hash=2,
            close=3
        }
    }
}
