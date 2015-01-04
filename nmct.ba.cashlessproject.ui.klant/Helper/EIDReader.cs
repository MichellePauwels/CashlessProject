using be.belgium.eid;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace nmct.ba.cashlessproject.ui.klant.Helper
{
    public class EIDReader
    {
        public EIDReader()
        {
            Init();
        }
        
        public static EID Init()
        {
            try
            {
                BEID_ReaderSet ReaderSet;
                ReaderSet = BEID_ReaderSet.instance();

                BEID_ReaderContext Reader;
                Reader = ReaderSet.getReader();

                
                string sText;
                sText = "Reader = " + Reader.getName() + "\r\n\r\n";

                if (Reader.isCardPresent())
                {
                    if (Reader.getCardType() == BEID_CardType.BEID_CARDTYPE_EID || Reader.getCardType() == BEID_CardType.BEID_CARDTYPE_FOREIGNER || Reader.getCardType() == BEID_CardType.BEID_CARDTYPE_KIDS)
                    {
                        EID eid= Load_eid(Reader);
                        return eid;
                    }
                    else
                    {
                        sText += "CARD TYPE UNKNOWN";
                    }
                }

                BEID_ReaderSet.releaseSDK();
            }

            catch (BEID_Exception ex)
            {
                BEID_ReaderSet.releaseSDK();
            }
            catch (Exception ex)
            {
                BEID_ReaderSet.releaseSDK();
            }
            return null;
        }

        public static EID Load_eid(BEID_ReaderContext Reader)
        {
            BEID_EIDCard card;
            card = Reader.getEIDCard();

            if (card.isTestCard())
            {
                card.setAllowTestCard(true);
            }

            BEID_EId doc;
            doc = card.getID();

            EID eid = new EID();
            eid.Firstname = doc.getFirstName();
            eid.Surname = doc.getSurname();
            eid.Rijksregisternummer = doc.getNationalNumber();
            eid.Street = doc.getStreet();
            eid.Country = doc.getMunicipality();

            Image photo;
            BEID_Picture picture;
            picture = card.getPicture();

            byte[] bytearray;
            bytearray = picture.getData().GetBytes();

            MemoryStream ms;
            ms = new MemoryStream();
            ms.Write(bytearray, 0, bytearray.Length);

            return eid;
        }
    }
}
