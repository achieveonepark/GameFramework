﻿using System;

namespace org.puremvc.csharp.interfaces
{
    /// <summary>
    /// The interface definition for a PureMVC Model
    /// </summary>
    /// <remarks>
    ///     <para>In PureMVC, <c>IModel</c> implementors provide access to <c>IProxy</c> objects by named lookup</para>
    ///     <para>An <c>IModel</c> assumes these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Maintain a cache of <c>IProxy</c> instances</item>
    ///         <item>Provide methods for registering, retrieving, and removing <c>IProxy</c> instances</item>
    ///     </list>
    /// </remarks>
    public interface IModel
    {
        /// <summary>
        /// Register an <c>IProxy</c> instance with the <c>Model</c>
        /// </summary>
        /// <param name="proxy">A reference to the proxy object to be held by the <c>Model</c></param>
		void registerProxy( IProxy proxy );

        /// <summary>
        /// Retrieve an <c>IProxy</c> instance from the Model
        /// </summary>
        /// <param name="proxyName">The name of the proxy to retrieve</param>
        /// <returns>The <c>IProxy</c> instance previously registered with the given <c>proxyName</c></returns>
		IProxy retrieveProxy( String proxyName );

        /// <summary>
        /// Remove an <c>IProxy</c> instance from the Model
        /// </summary>
        /// <param name="proxyName">The name of the <c>IProxy</c> instance to be removed</param>
		void removeProxy( String proxyName );
    }
}