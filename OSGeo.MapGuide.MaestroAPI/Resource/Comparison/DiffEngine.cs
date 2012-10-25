#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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

// Original code by Michael Potter, made available under Public Domain
//
// http://www.codeproject.com/Articles/6943/A-Generic-Reusable-Diff-Algorithm-in-C-II/
#endregion
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Comparison
{
    /// <summary>
    /// Defines a list of difference
    /// </summary>
    public interface IDiffList
    {
        /// <summary>
        /// Gets the number of instances
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Gets a diff at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IComparable GetByIndex(int index);
    }

    internal enum DiffStatus
    {
        Matched = 1,
        NoMatch = -1,
        Unknown = -2
    }

    internal class DiffState
    {
        private const int BAD_INDEX = -1;
        private int _startIndex;
        private int _length;

        public int StartIndex { get { return _startIndex; } }
        public int EndIndex { get { return ((_startIndex + _length) - 1); } }
        public int Length
        {
            get
            {
                int len;
                if (_length > 0)
                {
                    len = _length;
                }
                else
                {
                    if (_length == 0)
                    {
                        len = 1;
                    }
                    else
                    {
                        len = 0;
                    }
                }
                return len;
            }
        }

        public DiffStatus Status
        {
            get
            {
                DiffStatus stat;
                if (_length > 0)
                {
                    stat = DiffStatus.Matched;
                }
                else
                {
                    switch (_length)
                    {
                        case -1:
                            stat = DiffStatus.NoMatch;
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(_length == -2, "Invalid status: _length < -2"); //NOXLATE
                            stat = DiffStatus.Unknown;
                            break;
                    }
                }
                return stat;
            }
        }

        public DiffState()
        {
            SetToUnkown();
        }

        protected void SetToUnkown()
        {
            _startIndex = BAD_INDEX;
            _length = (int)DiffStatus.Unknown;
        }

        public void SetMatch(int start, int length)
        {
            System.Diagnostics.Debug.Assert(length > 0, "Length must be greater than zero"); //NOXLATE
            System.Diagnostics.Debug.Assert(start >= 0, "Start must be greater than or equal to zero"); //NOXLATE
            _startIndex = start;
            _length = length;
        }

        public void SetNoMatch()
        {
            _startIndex = BAD_INDEX;
            _length = (int)DiffStatus.NoMatch;
        }


        public bool HasValidLength(int newStart, int newEnd, int maxPossibleDestLength)
        {
            if (_length > 0) //have unlocked match
            {
                if ((maxPossibleDestLength < _length) ||
                    ((_startIndex < newStart) || (EndIndex > newEnd)))
                {
                    SetToUnkown();
                }
            }
            return (_length != (int)DiffStatus.Unknown);
        }
    }

    internal class DiffStateList
    {
#if USE_HASH_TABLE
		private Hashtable _table;
#else
        private DiffState[] _array;
#endif
        public DiffStateList(int destCount)
        {
#if USE_HASH_TABLE
			_table = new Hashtable(Math.Max(9,destCount/10));
#else
            _array = new DiffState[destCount];
#endif
        }

        public DiffState GetByIndex(int index)
        {
#if USE_HASH_TABLE
			DiffState retval = (DiffState)_table[index];
			if (retval == null)
			{
				retval = new DiffState();
				_table.Add(index,retval);
			}
#else
            DiffState retval = _array[index];
            if (retval == null)
            {
                retval = new DiffState();
                _array[index] = retval;
            }
#endif
            return retval;
        }
    }

    /// <summary>
    /// Defines the status of a diff result span
    /// </summary>
    public enum DiffResultSpanStatus
    {
        /// <summary>
        /// 
        /// </summary>
        NoChange,
        /// <summary>
        /// 
        /// </summary>
        Replace,
        /// <summary>
        /// 
        /// </summary>
        DeleteSource,
        /// <summary>
        /// 
        /// </summary>
        AddDestination
    }

    /// <summary>
    /// Defines a diff result span
    /// </summary>
    public class DiffResultSpan : IComparable
    {
        private const int BAD_INDEX = -1;
        private int _destIndex;
        private int _sourceIndex;
        private int _length;
        private DiffResultSpanStatus _status;

        /// <summary>
        /// The destination index
        /// </summary>
        public int DestIndex { get { return _destIndex; } }

        /// <summary>
        /// The source index
        /// </summary>
        public int SourceIndex { get { return _sourceIndex; } }

        /// <summary>
        /// Gets the length of this span
        /// </summary>
        public int Length { get { return _length; } }

        /// <summary>
        /// Gets the status of this span
        /// </summary>
        public DiffResultSpanStatus Status { get { return _status; } }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        /// <param name="status"></param>
        /// <param name="destIndex"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="length"></param>
        protected DiffResultSpan(
            DiffResultSpanStatus status,
            int destIndex,
            int sourceIndex,
            int length)
        {
            _status = status;
            _destIndex = destIndex;
            _sourceIndex = sourceIndex;
            _length = length;
        }

        internal static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.NoChange, destIndex, sourceIndex, length);
        }

        internal static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.Replace, destIndex, sourceIndex, length);
        }

        internal static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.DeleteSource, BAD_INDEX, sourceIndex, length);
        }

        internal static DiffResultSpan CreateAddDestination(int destIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.AddDestination, destIndex, BAD_INDEX, length);
        }

        internal void AddLength(int i)
        {
            _length += i;
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} (Dest: {1},Source: {2}) {3}", //NOXLATE
                _status.ToString(),
                _destIndex.ToString(),
                _sourceIndex.ToString(),
                _length.ToString());
        }
        #region IComparable Members
        /// <summary>
        /// Compares this instance against the specified object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return _destIndex.CompareTo(((DiffResultSpan)obj)._destIndex);
        }

        #endregion
    }

    /// <summary>
    /// Controls the level of perfection required when computing the differences
    /// </summary>
    public enum DiffEngineLevel
    {
        /// <summary>
        /// Fast, but imperfect
        /// </summary>
        FastImperfect,
        /// <summary>
        /// A balanced trade between speed and perfection
        /// </summary>
        Medium,
        /// <summary>
        /// Slow, but perfect
        /// </summary>
        SlowPerfect
    }

    /// <summary>
    /// Computes the differences between two sources
    /// </summary>
    public class DiffEngine
    {
        private IDiffList _source;
        private IDiffList _dest;
        private List<DiffResultSpan> _matchList;

        private DiffEngineLevel _level;

        private DiffStateList _stateList;

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public DiffEngine()
        {
            _source = null;
            _dest = null;
            _matchList = null;
            _stateList = null;
            _level = DiffEngineLevel.FastImperfect;
        }

        private int GetSourceMatchLength(int destIndex, int sourceIndex, int maxLength)
        {
            int matchCount;
            for (matchCount = 0; matchCount < maxLength; matchCount++)
            {
                if (_dest.GetByIndex(destIndex + matchCount).CompareTo(_source.GetByIndex(sourceIndex + matchCount)) != 0)
                {
                    break;
                }
            }
            return matchCount;
        }

        private void GetLongestSourceMatch(DiffState curItem, int destIndex, int destEnd, int sourceStart, int sourceEnd)
        {

            int maxDestLength = (destEnd - destIndex) + 1;
            int curLength = 0;
            int curBestLength = 0;
            int curBestIndex = -1;
            int maxLength = 0;
            for (int sourceIndex = sourceStart; sourceIndex <= sourceEnd; sourceIndex++)
            {
                maxLength = Math.Min(maxDestLength, (sourceEnd - sourceIndex) + 1);
                if (maxLength <= curBestLength)
                {
                    //No chance to find a longer one any more
                    break;
                }
                curLength = GetSourceMatchLength(destIndex, sourceIndex, maxLength);
                if (curLength > curBestLength)
                {
                    //This is the best match so far
                    curBestIndex = sourceIndex;
                    curBestLength = curLength;
                }
                //jump over the match
                sourceIndex += curBestLength;
            }
            //DiffState cur = _stateList.GetByIndex(destIndex);
            if (curBestIndex == -1)
            {
                curItem.SetNoMatch();
            }
            else
            {
                curItem.SetMatch(curBestIndex, curBestLength);
            }

        }

        private void ProcessRange(int destStart, int destEnd, int sourceStart, int sourceEnd)
        {
            int curBestIndex = -1;
            int curBestLength = -1;
            int maxPossibleDestLength = 0;
            DiffState curItem = null;
            DiffState bestItem = null;
            for (int destIndex = destStart; destIndex <= destEnd; destIndex++)
            {
                maxPossibleDestLength = (destEnd - destIndex) + 1;
                if (maxPossibleDestLength <= curBestLength)
                {
                    //we won't find a longer one even if we looked
                    break;
                }
                curItem = _stateList.GetByIndex(destIndex);

                if (!curItem.HasValidLength(sourceStart, sourceEnd, maxPossibleDestLength))
                {
                    //recalc new best length since it isn't valid or has never been done.
                    GetLongestSourceMatch(curItem, destIndex, destEnd, sourceStart, sourceEnd);
                }
                if (curItem.Status == DiffStatus.Matched)
                {
                    switch (_level)
                    {
                        case DiffEngineLevel.FastImperfect:
                            if (curItem.Length > curBestLength)
                            {
                                //this is longest match so far
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                            }
                            //Jump over the match 
                            destIndex += curItem.Length - 1;
                            break;
                        case DiffEngineLevel.Medium:
                            if (curItem.Length > curBestLength)
                            {
                                //this is longest match so far
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                                //Jump over the match 
                                destIndex += curItem.Length - 1;
                            }
                            break;
                        default:
                            if (curItem.Length > curBestLength)
                            {
                                //this is longest match so far
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                            }
                            break;
                    }
                }
            }
            if (curBestIndex < 0)
            {
                //we are done - there are no matches in this span
            }
            else
            {

                int sourceIndex = bestItem.StartIndex;
                _matchList.Add(DiffResultSpan.CreateNoChange(curBestIndex, sourceIndex, curBestLength));
                if (destStart < curBestIndex)
                {
                    //Still have more lower destination data
                    if (sourceStart < sourceIndex)
                    {
                        //Still have more lower source data
                        // Recursive call to process lower indexes
                        ProcessRange(destStart, curBestIndex - 1, sourceStart, sourceIndex - 1);
                    }
                }
                int upperDestStart = curBestIndex + curBestLength;
                int upperSourceStart = sourceIndex + curBestLength;
                if (destEnd > upperDestStart)
                {
                    //we still have more upper dest data
                    if (sourceEnd > upperSourceStart)
                    {
                        //set still have more upper source data
                        // Recursive call to process upper indexes
                        ProcessRange(upperDestStart, destEnd, upperSourceStart, sourceEnd);
                    }
                }
            }
        }

        /// <summary>
        /// Performs the difference computation using the specified level of control
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="level"></param>
        /// <returns>The total execution time in seconds</returns>
        public double ProcessDiff(IDiffList source, IDiffList destination, DiffEngineLevel level)
        {
            _level = level;
            return ProcessDiff(source, destination);
        }

        /// <summary>
        /// Performs the difference computation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>The total execution time in seconds</returns>
        public double ProcessDiff(IDiffList source, IDiffList destination)
        {
            DateTime dt = DateTime.Now;
            _source = source;
            _dest = destination;
            _matchList = new List<DiffResultSpan>();

            int dcount = _dest.Count();
            int scount = _source.Count();


            if ((dcount > 0) && (scount > 0))
            {
                _stateList = new DiffStateList(dcount);
                ProcessRange(0, dcount - 1, 0, scount - 1);
            }

            TimeSpan ts = DateTime.Now - dt;
            return ts.TotalSeconds;
        }


        private bool AddChanges(
            List<DiffResultSpan> report,
            int curDest,
            int nextDest,
            int curSource,
            int nextSource)
        {
            bool retval = false;
            int diffDest = nextDest - curDest;
            int diffSource = nextSource - curSource;
            int minDiff = 0;
            if (diffDest > 0)
            {
                if (diffSource > 0)
                {
                    minDiff = Math.Min(diffDest, diffSource);
                    report.Add(DiffResultSpan.CreateReplace(curDest, curSource, minDiff));
                    if (diffDest > diffSource)
                    {
                        curDest += minDiff;
                        report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest - diffSource));
                    }
                    else
                    {
                        if (diffSource > diffDest)
                        {
                            curSource += minDiff;
                            report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource - diffDest));
                        }
                    }
                }
                else
                {
                    report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest));
                }
                retval = true;
            }
            else
            {
                if (diffSource > 0)
                {
                    report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource));
                    retval = true;
                }
            }
            return retval;
        }

        /// <summary>
        /// Returns the result of the difference computation
        /// </summary>
        /// <returns></returns>
        public List<DiffResultSpan> DiffReport()
        {
            var retval = new List<DiffResultSpan>();
            int dcount = _dest.Count();
            int scount = _source.Count();

            //Deal with the special case of empty files
            if (dcount == 0)
            {
                if (scount > 0)
                {
                    retval.Add(DiffResultSpan.CreateDeleteSource(0, scount));
                }
                return retval;
            }
            else
            {
                if (scount == 0)
                {
                    retval.Add(DiffResultSpan.CreateAddDestination(0, dcount));
                    return retval;
                }
            }


            _matchList.Sort();
            int curDest = 0;
            int curSource = 0;
            DiffResultSpan last = null;

            //Process each match record
            foreach (DiffResultSpan drs in _matchList)
            {
                if ((!AddChanges(retval, curDest, drs.DestIndex, curSource, drs.SourceIndex)) &&
                    (last != null))
                {
                    last.AddLength(drs.Length);
                }
                else
                {
                    retval.Add(drs);
                }
                curDest = drs.DestIndex + drs.Length;
                curSource = drs.SourceIndex + drs.Length;
                last = drs;
            }

            //Process any tail end data
            AddChanges(retval, curDest, dcount, curSource, scount);

            return retval;
        }
    }
}
