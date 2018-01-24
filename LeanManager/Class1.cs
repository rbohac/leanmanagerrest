using System;
using System.Net;
using Grapevine.Server;
using QuantConnect.Interfaces;
using QuantConnect.Packets;
using Grapevine;
using QuantConnect.Algorithm;
using QuantConnect.Algorithm.CSharp;


namespace QuantConnect.Lean.Engine.Server
{
    /// <summary>
    /// NOP implementation of the ILeanManager interface
    /// </summary>
    public class RaysLeanManager : ILeanManager
    {

        public static GrapeAlgorithm Algorithm;
        /// <summary>
        /// Empty implementation of the ILeanManager interface
        /// </summary>
        /// <param name="systemHandlers">Exposes lean engine system handlers running LEAN</param>
        /// <param name="algorithmHandlers">Exposes the lean algorithm handlers running lean</param>
        /// <param name="job">The job packet representing either a live or backtest Lean instance</param>
        /// <param name="algorithmManager">The Algorithm manager</param>
        public void Initialize(LeanEngineSystemHandlers systemHandlers, LeanEngineAlgorithmHandlers algorithmHandlers, AlgorithmNodePacket job, AlgorithmManager algorithmManager)
        {
            // NOP
            var server = new RESTServer();
            //var server = new RESTServer("*");
            Console.WriteLine(String.Format("Core Router REST Server Listening On {0}:{1} @ {2}", server.Host, server.Port, server.BaseUrl));
            server.Start();
            Console.WriteLine("RaysLeanManager Initialized!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        /// <summary>
        /// Sets the IAlgorithm instance in the ILeanManager
        /// </summary>
        /// <param name="algorithm">The IAlgorithm instance being run</param>
        public void SetAlgorithm(IAlgorithm algorithm)
        {
            Algorithm = (GrapeAlgorithm)algorithm;
        }

        /// <summary>
        /// Update ILeanManager with the IAlgorithm instance
        /// </summary>
        public void Update()
        {
            // NOP
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            // NOP
        }

        
    }
}
