﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using System.Collections.Generic;

namespace osu.Framework.Configuration
{
    /// <summary>
    /// An readonly interface which can be bound to other <see cref="IBindableCollection{T}"/>s in order to watch for state and content changes.
    /// </summary>
    /// <typeparam name="T">The type of value encapsulated by this <see cref="IBindableCollection{T}"/>.</typeparam>
    public interface IBindableCollection<T> : IReadOnlyCollection<T>, IUnbindable, ICanBeDisabled
    {
        /// <summary>
        /// An event which is raised when an range of items get added.
        /// </summary>
        event Action<IEnumerable<T>> ItemsAdded;

        /// <summary>
        /// An event which is raised when items get removed.
        /// </summary>
        event Action<IEnumerable<T>> ItemsRemoved;

        /// <summary>
        /// Binds self to another bindable such that we receive any values and value limitations of the bindable we bind width.
        /// </summary>
        /// <param name="them">The foreign bindable. This should always be the most permanent end of the bind (ie. a ConfigManager)</param>
        void BindTo(IBindableCollection<T> them);

        /// <summary>
        /// Retrieve a new bindable instance weakly bound to the configuration backing.
        /// If you are further binding to events of a bindable retrieved using this method, ensure to hold
        /// a local reference.
        /// </summary>
        /// <returns>A weakly bound copy of the specified bindable.</returns>
        IBindableCollection<T> GetBoundCopy();
    }
}
