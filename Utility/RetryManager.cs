﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityHouse.Helper
{
    public static class RetryManager
    {
        private const int RetryCount = 3;
        private const int TimeToWait = 5000;

        public static void RetryExecute(Action action)
        {
            int retryCountLeft = RetryCount;
            do
            {
                try
                {
                    if (action != null)
                    {
                        action();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                catch (TimeoutException ex)
                {
                    if (retryCountLeft == 0)
                    {
                        throw;
                    }
                    Thread.Sleep(TimeToWait);
                }
            } while (retryCountLeft-- > 0);
        }

        public static T RetryExecute<T>(Func<T> action)
        {
            int retryCountLeft = RetryCount;
            do
            {
                try
                {
                    if (action != null)
                    {
                        return action();
                    }
                    else
                    {
                        return default(T);
                    }
                }
                catch (TimeoutException ex)
                {
                    if (retryCountLeft == 0)
                    {
                        throw;
                    }
                    Thread.Sleep(TimeToWait);
                }
            } while (retryCountLeft-- > 0);

            return default(T);
        }
    }
}
