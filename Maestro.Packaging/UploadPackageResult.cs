#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Maestro.Packaging
{
    /// <summary>
    /// Represents the result of a non-transactional package loading operation
    /// </summary>
    public class UploadPackageResult
    {
        /// <summary>
        /// Gets the successful operations
        /// </summary>
        public ICollection<PackageOperation> Successful { get; private set; }

        /// <summary>
        /// Gets the failed operations
        /// </summary>
        public Dictionary<PackageOperation, Exception> Failed { get; private set; }

        /// <summary>
        /// Gets the operations that were skipped
        /// </summary>
        public ICollection<PackageOperation> SkipOperations { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadPackageResult"/> class.
        /// </summary>
        public UploadPackageResult() : this(new PackageOperation[0]) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadPackageResult"/> class.
        /// </summary>
        /// <param name="skip">The skip.</param>
        public UploadPackageResult(IEnumerable<PackageOperation> skip)
        {
            this.Successful = new List<PackageOperation>();
            this.Failed = new Dictionary<PackageOperation, Exception>();
            this.SkipOperations = new List<PackageOperation>(skip);
        }
    }
}
