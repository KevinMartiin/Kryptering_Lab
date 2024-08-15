using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppLabKryptering
{
    public class EncryptedData
    {
        public int Id { get; set; }  // Unikt ID för varje post
        public string EncryptedText { get; set; }  // Krypterad text
        public byte[] Salt { get; set; }  // Saltet som används för kryptering
    }
}
