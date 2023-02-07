using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Extending.Configuring.Manager;

namespace Apizr
{
    public static class MediationFileTransferOptionsBuilderExtensions
    {
        /// <summary>
        /// Let Apizr handle file transfer requests execution with some mediation
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IApizrExtendedManagerOptionsBuilder WithFileTransferMediation(this IApizrExtendedManagerOptionsBuilder optionsBuilder)
        {
            WithMediation(optionsBuilder.ApizrOptions);

            return optionsBuilder;
        }
    }
}
