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
            //...
            mitemRepre.TrainData();
            this.learn(mitemRepre);
        }

        protected static float[,] tempResult = new float[9, 1];

        public float[,] gettempResult()
        {
            return tempResult;
        }

        public void learn(ItemRepre tempRepre)
        {
            for (int i = 0; i < 9; i++)
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
