# 1 "examsum.c"
# 1 "<built-in>"
# 1 "<command-line>"
# 1 "examsum.c"
void main(void)
{
  long i, k, sum=0;
  for(k=1; k <= 100; k++)
    for(i=1; i <= 1000000; i++)
      sum += i;
}
