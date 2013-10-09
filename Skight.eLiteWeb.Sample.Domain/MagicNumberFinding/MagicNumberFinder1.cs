using System;

namespace Skight.HelpCenter.Domain
{
    
    public class MagicNumberFinder1
    {
     
        public void find()
{
for(int i = 1; i < 10;i++) //1
{
if(i%1!=0)
{
continue;
}else
{
for(int j = 1; j < 10; j++)//2
{
if(i==j)
{
continue;
}else
{
if((i*10 + j)%2 == 0)
{
for(int k = 1; k < 10 ; k++)//3
{
if((k==i)||(k==j))
{
continue;
}else
{
if((i*100 + j*10 + k)%3==0)
{
for(int l = 1 ;l <10;l++)//4
{
if((l==i)||(l==j)||(l==k))
{
continue;
}else
{
if((i*1000+j*100+k*10+l)%4==0)
{
for(int m = 1;m <10;m++)//5
{
if((m==i)||(m==j)||(m==k)||(m==l))
{
continue;
}else
{
if((i*10000+j*1000+k*100+l*10+m)%5==0)
{
for(int n=1;n<10;n++)//6
{
if((n==i)||(n==j)||(n==k)||(n==l)||(n==m))
{
continue;
}else
{
if((i*100000+j*10000+k*1000+l*100+m*10+n)%6==0)
{
for(int x = 1;x < 10; x++)//7
{
if((x==i)||(x==j)||(x==k)||(x==l)||(x==m)||(x==n))
{
continue;
}else
{
if((i*1000000+j*100000+k*10000+l*1000+m*100+10*n+x)%7==0)
{
for(int y =0; y < 10;y++)//8
{
if((y==i)||(y==j)||(y==k)||(y==l)||(y==m)||(y==n)||(y==x))
{
continue;
}else
{
if((i*10000000+j*1000000+k*100000+l*10000+m*1000+n*100+10*x+y)%8==0)
{
for(int z = 1; z < 10; z++)
{
if((z==i)||(z==j)||(z==k)||(z==l)||(z==m)||(z==n)||(z==x)||(z==y))
{
continue;
}else
{
if((i*100000000+j*10000000+k*1000000+l*100000+m*10000+n*1000+x*100+10*y+z)%9==0)
{
Console.WriteLine(i+""+j+""+k+""+l+""+m+""+n+""+x+""+y+""+z);
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
}
    }
}