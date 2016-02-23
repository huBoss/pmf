using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMF
{
    class GenData
    { 
        public void randomdata(double[,]errormap,double[,]sig,double[]volt,int bulks,int bufSize,int ynum,int znum)
        {
            Random rnd = new Random();
            for (int i=0;i<8;++i)
                for (int j=0;j<bulks*bufSize;++j)
                {
                    sig[i, j] = rnd.Next(1000)/5.0;
                }
            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 5; ++j)
                    errormap[i, j] = 0;
            for (int i = 5; i < ynum; ++i)
                for (int j = 5; j < znum; ++j)
                {
                    errormap[i, j] = rnd.Next(1000) / 5.0 +0.1;
                }
            for (int i=0;i<8;++i)
            {
                volt[i] = rnd.NextDouble();
            }
        }
    }
}
