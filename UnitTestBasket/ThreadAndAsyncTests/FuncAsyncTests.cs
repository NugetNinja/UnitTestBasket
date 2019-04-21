using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAndAsyncTests
{
    [TestClass]
    public class FuncAsyncTests
    {
        object objLock = new object();

        /// <summary>
        /// ���� Func �첽�����ĵȴ����
        /// </summary>
        [TestMethod]
        public async Task FuncAsyncTest()
        {
            int index = 0;//�߳���ɼ���
            var testCount = 5;//���������߳���
            var delayMs = 300;//�߳��ӳٺ�����
            var lastRunTime = SystemTime.Now;
            Func<DateTimeOffset,Task<int>> funcAsyncTest = async time =>
            {
                lastRunTime = time;//��¼��ǰʱ��
                Console.WriteLine($"{SystemTime.Now.ToString("ss.ffff")}\t���� funcAsyncTest ");
                await Task.Delay(delayMs);//�ӳ�

                Console.WriteLine($"{SystemTime.Now.ToString("ss.ffff")}\t[{index}]�ӳٽ�������ʼִ�� funcAsyncTest ");

                lock (objLock)
                {
                    index++;
                }
                return index;
            };

            Console.WriteLine("==== ���ȴ� ====");
            for (int i = 0; i < testCount; i++)
            {
                _ = funcAsyncTest(SystemTime.Now);//���ȴ���ֱ��ִ����һ��
                Assert.IsTrue((SystemTime.Now - lastRunTime).TotalMilliseconds < delayMs);//ѭ�����ʱ��ǳ���
            }

            while (index < testCount)
            {
                //�ȴ��߳�ִ����ϣ����ﲻʹ��Task.Wait()�Լ��߳��źŵƣ�
            }

            index = 0;
            Console.WriteLine("==== �ȴ� ====");

            for (int i = 0; i < testCount; i++)
            {
                await funcAsyncTest(SystemTime.Now);//����еȴ�
                Assert.IsTrue((SystemTime.Now - lastRunTime).TotalMilliseconds >= delayMs);//ѭ�����ʱ��ȡ�����߳����ӳ����
            }

            while (index < testCount)
            {
                //�ȴ��߳�ִ����ϣ����ﲻʹ��Task.Wait()�Լ��߳��źŵƣ�
            }
        }
    }
}
