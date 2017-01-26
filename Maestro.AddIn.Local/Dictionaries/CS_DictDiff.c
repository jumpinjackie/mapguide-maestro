/*
 * Copyright (c) 2008, Autodesk, Inc.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the Autodesk, Inc. nor the names of its
 *       contributors may be used to endorse or promote products derived
 *       from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY Autodesk, Inc. ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL Autodesk, Inc. OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#include "cs_map.h"

static void Usage (void);

int main (int argc,char *argv [])
{
	extern char cs_Dir [];
	extern char* cs_DirP;
	extern char cs_Csname [];
	extern char cs_Dtname [];
	extern char cs_Elname [];

	int cmpVal, iTmp;
	int csDiffCnt, dtDiffCnt, elDiffCnt;
	int dummy;
	int nlFlag;

	csFILE *wasStrm, *isStrm;

	char wasDir [MAXPATH];
	char isDir [MAXPATH];
	char errorText [260];

	struct cs_Csdef_ wasCsDef, isCsDef;
	struct cs_Dtdef_ wasDtDef, isDtDef;
	struct cs_Eldef_ wasElDef, isElDef;

	csDiffCnt = 0;
	dtDiffCnt = 0;
	elDiffCnt = 0;

	if (argc != 3) Usage ();

	strncpy (wasDir,argv [1],sizeof (wasDir));
	wasDir [sizeof (wasDir) - 1] = '\0';

	strncpy (isDir,argv [2],sizeof (isDir));
	isDir [sizeof (isDir) - 1] = '\0';

	if (CS_altdr (wasDir) != 0)
	{
		printf ("%s file not found in %s directory.\n",cs_Csname,wasDir);
		Usage ();
	}
	CS_stcpy (cs_DirP,cs_Dtname);
	if (CS_access (cs_Dir,4) != 0)
	{
		printf ("%s file not found in %s directory.\n",cs_Dtname,wasDir);
		Usage ();
	}
	CS_stcpy (cs_DirP,cs_Elname);
	if (CS_access (cs_Dir,4) != 0)
	{
		printf ("%s file not found in %s directory.\n",cs_Elname,wasDir);
		Usage ();
	}

	if (CS_altdr (isDir) != 0)
	{
		printf ("%s file not found in %s directory.\n",cs_Csname,isDir);
		Usage ();
	}
	CS_stcpy (cs_DirP,cs_Dtname);
	if (CS_access (cs_Dir,4) != 0)
	{
		printf ("%s file not found in %s directory.\n",cs_Dtname,isDir);
		Usage ();
	}
	CS_stcpy (cs_DirP,cs_Elname);
	if (CS_access (cs_Dir,4) != 0)
	{
		printf ("%s file not found in %s directory.\n",cs_Elname,isDir);
		Usage ();
	}

	/* Ok, we're ready to do our thing.  Do coordinate systems first. */
	CS_altdr (wasDir);											/*lint !e534 */
	wasStrm = CS_csopn (_STRM_BINRD);
	if (wasStrm == NULL)
	{
		printf ("Open of %s in the 'WAS' directory failed.\n",cs_Csname);
		Usage ();
	}

	CS_altdr (isDir);											/*lint !e534 */
	isStrm = CS_csopn (_STRM_BINRD);
	if (isStrm == NULL)
	{
		printf ("Open of %s in the 'IS' directory failed.\n",cs_Csname);
		Usage ();
	}

	/* Now, we loop through the dictionaries.  In this module, we simply
	   compare key names, looking for new definitions, or deleted
	   definitions.  When we have definitions with the same name, we call
	   CS_csDiff to report any differences.

	   First, we prime the pump. */
	iTmp = CS_csrd (wasStrm,&wasCsDef,&dummy);
	if (iTmp < 0)
	{
		CS_errmsg (errorText,sizeof (errorText));
		printf ("%s\n",errorText);
		Usage ();
	}
	if (iTmp == 0)
	{
		printf ("'WAS' coordinate system dictionary is empty.\n");
		Usage ();
	}

	iTmp = CS_csrd (isStrm,&isCsDef,&dummy);
	if (iTmp < 0)
	{
		CS_errmsg (errorText,sizeof (errorText));
		printf ("%s\n",errorText);
		Usage ();
	}
	if (iTmp == 0)
	{
		printf ("'IS' coordinate system dictionary is empty.\n");
		Usage ();
	}

	nlFlag = FALSE;
	while (TRUE)
	{
		cmpVal = CS_stricmp (wasCsDef.key_nm,isCsDef.key_nm);
		if (cmpVal < 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s deleted from coordinate system dictionary!!!\n",wasCsDef.key_nm);
			csDiffCnt += 1;
		}
		else if (cmpVal > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s added to coordinate system dictionary!!!\n",isCsDef.key_nm);
			csDiffCnt += 1;
		}
		else
		{
			/* Normal case, we have two definitions with the same name, */
			iTmp = CS_csDiff (stdout,&wasCsDef,&isCsDef);
			if (iTmp != 0)
			{
				csDiffCnt += 1;
				nlFlag = TRUE;
			}
		}
		if (cmpVal >= 0)
		{
			iTmp = CS_csrd (isStrm,&isCsDef,&dummy);
			if (iTmp < 0)
			{
				CS_errmsg (errorText,sizeof (errorText));
				printf ("%s\n",errorText);
				Usage ();
			}
			if (iTmp == 0)
			{
				CS_fclose (isStrm);
				isStrm = NULL;
				break;
			}
		}
		if (cmpVal <= 0)
		{
			iTmp = CS_csrd (wasStrm,&wasCsDef,&dummy);
			if (iTmp < 0)
			{
				CS_errmsg (errorText,sizeof (errorText));
				printf ("%s\n",errorText);
				Usage ();
			}
			if (iTmp == 0)
			{
				CS_fclose (wasStrm);
				wasStrm = NULL;
				break;
			}
		}
	}

	/* Finish up properly. */
	nlFlag = TRUE;
	while (wasStrm != NULL)
	{
		iTmp = CS_csrd (wasStrm,&wasCsDef,&dummy);
		if (iTmp > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s deleted from coordinate system dictionary!!!\n",wasCsDef.key_nm);
			csDiffCnt += 1;
		}
		else if (iTmp == 0)
		{
			CS_fclose (wasStrm);
			wasStrm = NULL;
		}
		else
		{
			CS_errmsg (errorText,sizeof (errorText));
			printf ("%s\n",errorText);
			Usage ();
		}
	}
	while (isStrm != NULL)
	{
		iTmp = CS_csrd (isStrm,&wasCsDef,&dummy);
		if (iTmp > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s added to coordinate system dictionary!!!\n",isCsDef.key_nm);
			csDiffCnt += 1;
		}
		else if (iTmp == 0)
		{
			CS_fclose (isStrm);
			isStrm = NULL;
		}
		else
		{
			CS_errmsg (errorText,sizeof (errorText));
			printf ("%s\n",errorText);
			Usage ();
		}
	}
	if (csDiffCnt == 0)
	{
		printf ("\nCoordinate system dictionaries are the same.\n\n");
	}
	else
	{
		printf ("\n%d coordinate systems are different.\n\n",csDiffCnt);
	}

	/* Done with the coordinate system dictionary. Same as above, but with the
	   Datum Dictionary this time. */
	CS_altdr (wasDir);											/*lint !e534 */
	wasStrm = CS_dtopn (_STRM_BINRD);
	if (wasStrm == NULL)
	{
		printf ("Open of %s in the 'WAS' directory failed.\n",cs_Dtname);
		Usage ();
	}

	CS_altdr (isDir);											/*lint !e534 */
	isStrm = CS_dtopn (_STRM_BINRD);
	if (isStrm == NULL)
	{
		printf ("Open of %s in the 'IS' directory failed.\n",cs_Dtname);
		Usage ();
	}

	iTmp = CS_dtrd (wasStrm,&wasDtDef,&dummy);
	if (iTmp < 0)
	{
		CS_errmsg (errorText,sizeof (errorText));
		printf ("%s\n",errorText);
		Usage ();
	}
	if (iTmp == 0)
	{
		printf ("'WAS' datum dictionary is empty.\n");
		Usage ();
	}

	iTmp = CS_dtrd (isStrm,&isDtDef,&dummy);
	if (iTmp < 0)
	{
		CS_errmsg (errorText,sizeof (errorText));
		printf ("%s\n",errorText);
		Usage ();
	}
	if (iTmp == 0)
	{
		printf ("'IS' datum dictionary is empty.\n");
		Usage ();
	}

	nlFlag = FALSE;
	while (TRUE)
	{
		cmpVal = CS_stricmp (wasDtDef.key_nm,isDtDef.key_nm);
		if (cmpVal < 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s deleted from datum dictionary!!!\n",wasDtDef.key_nm);
			dtDiffCnt += 1;
		}
		else if (cmpVal > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s added to datum dictionary!!!\n",isDtDef.key_nm);
			dtDiffCnt += 1;
		}
		else
		{
			/* Normal case, we have two definitions with the same name, */
			iTmp = CS_dtDiff (stdout,&wasDtDef,&isDtDef);
			if (iTmp != 0)
			{
				dtDiffCnt += 1;
				nlFlag = TRUE;
			}
		}
		if (cmpVal >= 0)
		{
			iTmp = CS_dtrd (isStrm,&isDtDef,&dummy);
			if (iTmp < 0)
			{
				CS_errmsg (errorText,sizeof (errorText));
				printf ("%s\n",errorText);
				Usage ();
			}
			if (iTmp == 0)
			{
				CS_fclose (isStrm);
				isStrm = NULL;
				break;
			}
		}
		if (cmpVal <= 0)
		{
			iTmp = CS_dtrd (wasStrm,&wasDtDef,&dummy);
			if (iTmp < 0)
			{
				CS_errmsg (errorText,sizeof (errorText));
				printf ("%s\n",errorText);
				Usage ();
			}
			if (iTmp == 0)
			{
				CS_fclose (wasStrm);
				wasStrm = NULL;
				break;
			}
		}
	}

	nlFlag = TRUE;
	while (wasStrm != NULL)
	{
		iTmp = CS_dtrd (wasStrm,&wasDtDef,&dummy);
		if (iTmp > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s deleted from datum dictionary!!!\n",wasDtDef.key_nm);
			dtDiffCnt += 1;
		}
		else if (iTmp == 0)
		{
			CS_fclose (wasStrm);
			wasStrm = NULL;
		}
		else
		{
			CS_errmsg (errorText,sizeof (errorText));
			printf ("%s\n",errorText);
			Usage ();
		}
	}
	while (isStrm != NULL)
	{
		iTmp = CS_dtrd (isStrm,&wasDtDef,&dummy);
		if (iTmp > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s added to datum dictionary!!!\n",isDtDef.key_nm);
			dtDiffCnt += 1;
		}
		else if (iTmp == 0)
		{
			CS_fclose (isStrm);
			isStrm = NULL;
		}
		else
		{
			CS_errmsg (errorText,sizeof (errorText));
			printf ("%s\n",errorText);
			Usage ();
		}
	}
	if (dtDiffCnt == 0)
	{
		printf ("\nDatum dictionaries are the same.\n\n");
	}
	else
	{
		printf ("\n%d datum definitions are different.\n\n",dtDiffCnt);
	}

	/* Ellipsoid Dictionary */
	CS_altdr (wasDir);											/*lint !e534 */
	wasStrm = CS_elopn (_STRM_BINRD);
	if (wasStrm == NULL)
	{
		printf ("Open of %s in the 'WAS' directory failed.\n",cs_Elname);
		Usage ();
	}

	CS_altdr (isDir);											/*lint !e534 */
	isStrm = CS_elopn (_STRM_BINRD);
	if (isStrm == NULL)
	{
		printf ("Open of %s in the 'IS' directory failed.\n",cs_Elname);
		Usage ();
	}

	iTmp = CS_elrd (wasStrm,&wasElDef,&dummy);
	if (iTmp < 0)
	{
		CS_errmsg (errorText,sizeof (errorText));
		printf ("%s\n",errorText);
		Usage ();
	}
	if (iTmp == 0)
	{
		printf ("'WAS' ellipsoid dictionary is empty.\n");
		Usage ();
	}

	iTmp = CS_elrd (isStrm,&isElDef,&dummy);
	if (iTmp < 0)
	{
		CS_errmsg (errorText,sizeof (errorText));
		printf ("%s\n",errorText);
		Usage ();
	}
	if (iTmp == 0)
	{
		printf ("'IS' ellipsoid dictionary is empty.\n");
		Usage ();
	}

	nlFlag = FALSE;
	while (TRUE)
	{
		cmpVal = CS_stricmp (wasElDef.key_nm,isElDef.key_nm);
		if (cmpVal < 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s deleted from ellipsoid dictionary!!!\n",wasElDef.key_nm);
			elDiffCnt += 1;
		}
		else if (cmpVal > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s added to ellipsoid dictionary!!!\n",isElDef.key_nm);
			elDiffCnt += 1;
		}
		else
		{
			/* Normal case, we have two definitions with the same name, */
			iTmp = CS_elDiff (stdout,&wasElDef,&isElDef);
			if (iTmp != 0)
			{
				elDiffCnt += 1;
				nlFlag = TRUE;
			}
		}
		if (cmpVal >= 0)
		{
			iTmp = CS_elrd (isStrm,&isElDef,&dummy);
			if (iTmp < 0)
			{
				CS_errmsg (errorText,sizeof (errorText));
				printf ("%s\n",errorText);
				Usage ();
			}
			if (iTmp == 0)
			{
				CS_fclose (isStrm);
				isStrm = NULL;
				break;
			}
		}
		if (cmpVal <= 0)
		{
			iTmp = CS_elrd (wasStrm,&wasElDef,&dummy);
			if (iTmp < 0)
			{
				CS_errmsg (errorText,sizeof (errorText));
				printf ("%s\n",errorText);
				Usage ();
			}
			if (iTmp == 0)
			{
				CS_fclose (wasStrm);
				wasStrm = NULL;
				break;
			}
		}
	}

	nlFlag = TRUE;
	while (wasStrm != NULL)
	{
		iTmp = CS_elrd (wasStrm,&wasElDef,&dummy);
		if (iTmp > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s deleted from ellipsoid dictionary!!!\n",wasElDef.key_nm);
			elDiffCnt += 1;
		}
		else if (iTmp == 0)
		{
			CS_fclose (wasStrm);
			wasStrm = NULL;
		}
		else
		{
			CS_errmsg (errorText,sizeof (errorText));
			printf ("%s\n",errorText);
			Usage ();
		}
	}
	while (isStrm != NULL)
	{
		iTmp = CS_elrd (isStrm,&wasElDef,&dummy);
		if (iTmp > 0)
		{
			if (nlFlag)
			{
				printf ("\n");
				nlFlag = FALSE;
			}
			printf ("%s added to ellipsoid dictionary!!!\n",isElDef.key_nm);
			dtDiffCnt += 1;
		}
		else if (iTmp == 0)
		{
			CS_fclose (isStrm);
			isStrm = NULL;
		}
		else
		{
			CS_errmsg (errorText,sizeof (errorText));
			printf ("%s\n",errorText);
			Usage ();
		}
	}
	if (elDiffCnt == 0)
	{
		printf ("\nEllipsoid dictionaries are the same.\n\n");
	}
	else
	{
		printf ("\n%d ellipsoid definitions are different.\n\n",elDiffCnt);
	}

	return 0;
}
void Usage (void)
{
	printf ("Usage: DictDiff previous_directory new_directory\n");
	exit (1);
}
