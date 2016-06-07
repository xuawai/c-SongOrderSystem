using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;      

namespace KTV
{
    class ProfileLearn
    {
        public ProfileLearn()
        {
            ItemRepre mitemRepre = new ItemRepre();
            //set Cap and List<song>
            //....
            mitemRepre.TrainData();
            this.learn(mitemRepre);
        }

        public static float[,] tempResult = new float[RecomGenerate.singerLength+RecomGenerate.typeLength+RecomGenerate.languageLength, 1];

        public float[,] gettempResult()
        {
            return tempResult;
        }

        public void learn(ItemRepre tempRepre)
        {
            for (int i = 0; i < tempResult.GetLength(0); i++)
                {
                    float tempAdd = 0;
                    for (int j = 0; j < tempRepre.getRep().GetLength(1); j++)
                    {
                        tempAdd += tempRepre.getRep()[i, j];
                    }
                    tempResult[i, 0] = tempAdd / tempRepre.getRep().GetLength(1);
                 }
         }

     }
}
