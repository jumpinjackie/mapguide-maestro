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

#if _RUN_TIME < _rt_UNIXPCC
#	include <conio.h>		/* for acknowledgment */
#else
#       include <unistd.h>
#       include <ctype.h>
#       include <limits.h>  
#endif

#if defined (__TURBOC__) && defined (__MSDOS__)
unsigned _stklen = 16384;		/* was once required, probably not anymore  */
#endif 

void usage (void);
void acknowledge (void);
int err_disp (char *mesg);
int cs_Batch;

int main (int argc,char *argv [])
{
    //debug parameters
    ///b  sourceDir compiledDir
	extern char cs_Dir [];
	extern char *cs_DirP;
	extern char cs_Csname [];
	extern char cs_Ctname [];
	extern char cs_Dtname [];
	extern char cs_Elname [];
	extern char cs_Gpname [];
	extern char cs_Gxname [];
	extern char cs_OptchrC;
	extern char cs_DirsepC;
	extern int cs_Sortbs;
	extern union cs_Bswap_ cs_BswapU;

	int ii;
	int st;

	int batch;
	int crypt;
	int demo;
	int extents;
	int test;
	int warn;
	int flags;
	int err_cnt;

	char *cp;

	char src_dir [MAXPATH];
	char dst_dir [MAXPATH];
	char src_name [MAXPATH];
	char el_path [MAXPATH];
	char dt_path [MAXPATH];
	char cs_path [MAXPATH];
	char ct_path [MAXPATH];
	char mr_path [MAXPATH];
	char gp_path [MAXPATH];
	char gx_path [MAXPATH];

#if _MEM_MODEL == _mm_VIRTUAL
	cs_Sortbs = (128 * 1024);
#endif

	/* The following is necessary to set up the target directory
	   of the results. A NULL argument says see if the
	   environmental variable is set. The null string says
	   check the current directory. In any case, if a COORDSYS
	   is not found, cs_Dir is initialized to the current
	   directory, even in the event of an error. */

	st = CS_altdr (NULL);
	if (st != 0) CS_altdr ("");

	/* Process the arguments on the command line. */
	crypt = TRUE;
	batch = FALSE;
	cs_Batch = FALSE;
	test = FALSE;
	demo = FALSE;
	extents = TRUE;			/* Extents are normally on, option turns
							   them off. */
	warn = FALSE;
	src_dir [0] = '\0';
	dst_dir [0] = '\0';
	for (ii = 1;ii < argc;ii++)
	{
		cp = argv [ii];
		if (*cp == cs_OptchrC)
		{
			/* Here if the current argument is an
			   option. */

			cp += 1;
			if (*cp == 'b' || *cp == 'B')
			{
				batch = TRUE;
				cs_Batch = TRUE;
			}
			else if (*cp == 'c' || *cp == 'C')
			{
				crypt = FALSE;
			}
			else if (*cp == 'd' || *cp == 'D')
			{
				demo = TRUE;
			}
			else if (*cp == 'e' || *cp == 'E')
			{
				extents = FALSE;
			}
			else if (*cp == 's' || *cp == 'S')
			{
				/* Force CS_bswap to think that it is running
				   on a big endian machine, thus causing
				   the results to be written in big
				   endian, regardless of the system
				   upon which this program was compiled. */

				cs_BswapU.cccc [0] = 0x03;
				cs_BswapU.cccc [1] = 0x02;
				cs_BswapU.cccc [2] = 0x01;
				cs_BswapU.cccc [3] = 0x00;
			}
			else if (*cp == 't' || *cp == 'T')
			{
				test = TRUE;
			}
			else if (*cp == 'w' || *cp == 'W')
			{
				warn = TRUE;
			}
			else
			{
				printf ("Unknown option: %c\n",*cp);
				usage ();
				if (!batch) acknowledge ();
				return (1);
			}
			continue;
		}

		/* If it isn't an option, it must be the next
		   positional argument. */
		if (src_dir [0] == '\0')
		{
			/* We haven't captured a source directory
			   argument yet, so this argument must be
			   the source file name. */
			CS_stncp (src_dir,cp,sizeof (src_dir));
		}
		else if (dst_dir [0] == '\0')
		{
			/* We've captured a source directory name, so this
			   positional argument must be the destination
			   directory. */
			(void)CS_stncp (dst_dir,cp,sizeof (dst_dir));
		}
		else
		{
			/* We've seen a source directory and a destination
			   directory name.  Don't know what this argument
			   would be. */
			printf ("Two many positional arguments (%-16.16s)\n",argv [ii]);
			usage ();
			if (!batch) acknowledge ();
			return (1);
		}
	}

	/* Adjust as necessary. */
	if (src_dir [0] != '\0')
	{
		cp = src_dir + strlen (src_dir);
		if (*(cp - 1) != cs_DirsepC)
		{
			*cp++ = cs_DirsepC;
			*cp = '\0';
		}
	}

	if (dst_dir [0] != '\0')
	{
		cp = CS_stcpy (cs_Dir,dst_dir);
		if (*(cp - 1) != cs_DirsepC)
		{
			*cp++ = cs_DirsepC;
			*cp = '\0';
		}
		cs_DirP = cp;
	}
	flags = 0;
	if (crypt)   flags |= cs_CMPLR_CRYPT;
	if (demo)    flags |= cs_CMPLR_DEMO;
	if (test)    flags |= cs_CMPLR_TEST;
	if (warn)    flags |= cs_CMPLR_WARN;
	if (extents) flags |= cs_CMPLR_EXTENTS;
	strcpy (cs_DirP,cs_Elname);
	strcpy (el_path,cs_Dir);
	strcpy (cs_DirP,cs_Dtname);
	strcpy (dt_path,cs_Dir);
	strcpy (cs_DirP,cs_Csname);
	strcpy (cs_path,cs_Dir);
	strcpy (cs_DirP,cs_Ctname);
	strcpy (ct_path,cs_Dir);
	strcpy (cs_DirP,cs_Ctname);
	strcpy (ct_path,cs_Dir);
	strcpy (cs_DirP,cs_Gxname);
	strcpy (gx_path,cs_Dir);
	strcpy (cs_DirP,cs_Gpname);
	strcpy (gp_path,cs_Dir);
	*cs_DirP = '\0';
	strcpy (mr_path,cs_Dir);

	/* Compile the Ellipsoid Dictionary. */
	strcpy (src_name,src_dir);
	strcat (src_name,"elipsoid.asc");
	printf ("Compiling %s to %s.\n",src_name,el_path);
	err_cnt = CSelcomp (src_name,el_path,flags,err_disp);
	if (err_cnt != 0)
	{
		printf ("Compilation of %s failed, %s removed.\n",src_name,el_path);
		if (!batch) acknowledge ();
		return (1);
	}

	/* Compile the Datum Dictionary. */
	strcpy (src_name,src_dir);
	strcat (src_name,"datums.asc");
	printf ("Compiling %s to %s.\n",src_name,dt_path);
	err_cnt = CSdtcomp (src_name,dt_path,flags,el_path,err_disp);
	if (err_cnt != 0)
	{
		printf ("Compilation of %s failed, %s removed.\n",src_name,dt_path);
		if (!batch) acknowledge ();
		return (1);
	}

	/* Compile the Coordinate System Dictionary. */
	strcpy (src_name,src_dir);
	strcat (src_name,"coordsys.asc");
	printf ("Compiling %s to %s.\n",src_name,cs_path);
	err_cnt = CScscomp (src_name,cs_path,flags,el_path,dt_path,err_disp);
	if (err_cnt != 0)
	{
		printf ("Compilation of %s failed, %s removed.\n",src_name,cs_path);
		if (!batch) acknowledge ();
		return (1);
	}

	/* Compile the Category Dictionary. */
	strcpy (src_name,src_dir);
	strcat (src_name,"category.asc");
	printf ("Compiling %s to %s.\n",src_name,ct_path);
	err_cnt = CSctcomp (src_name,ct_path,flags,cs_path,err_disp);
	if (err_cnt != 0)
	{
		printf ("Compilation of %s failed, %s removed.\n",src_name,ct_path);
		if (!batch) acknowledge ();
		return (1);
	}

	/* Compile the Geodetic Transformation Dictionary. */
	strcpy (src_name,src_dir);
	strcat (src_name,"GeodeticTransformation.asc");
	printf ("Compiling %s to %s.\n",src_name,gx_path);
	err_cnt = CSgxcomp (src_name,gx_path,flags,dt_path,err_disp);
	if (err_cnt != 0)
	{
		printf ("Compilation of %s failed, %s removed.\n",src_name,gp_path);
		if (!batch) acknowledge ();
		return (1);
	}

	/* Compile the Geodetic Path Dictionary. */
	strcpy (src_name,src_dir);
	strcat (src_name,"GeodeticPath.asc");
	printf ("Compiling %s to %s.\n",src_name,gp_path);
	err_cnt = CSgpcomp (src_name,gp_path,flags,dt_path,gx_path,err_disp);
	if (err_cnt != 0)
	{
		printf ("Compilation of %s failed, %s removed.\n",src_name,gp_path);
		if (!batch) acknowledge ();
		return (1);
	}


	/* We're done. */
	printf ("All dictionaries and multiple regressions compiled successfully.\n");
	if (!batch) acknowledge ();

	return (0);
}

int err_disp (char *mesg)
{
	extern int cs_Batch;

	int cc;

	cc = '\0';
	if (!CS_strnicmp (mesg,"Warning",7))
	{
		printf ("\r%s\n",mesg);
	}
	else
	{
		printf ("\rError: %s\n",mesg);
	}
	if (!cs_Batch)
	{
#if _RUN_TIME < _rt_UNIXPCC
		/* Use this if kbhit/getch are available. */
		printf ("Press C to cancel, any other key to continue: ");
		while (kbhit ()) getch ();
		while (!kbhit ());
		cc = getch ();
		printf ("\n");
#elif __cplusplus
		/* Use this if kbhit/getch not available and C++ */
		char strbuf [MAX_INPUT];
		cout << "Press any key to continue: " << flush;
		cin.getline(strbuf, MAX_INPUT, '\n');
		cout << "\n" << flush;
		cc = (int)(strbuf[0]);
#else
		/* Use this as last resort. Note, no flush!!! */
		printf ("Press C to cancel, any other key to continue: ");
		cc = getc (stdin);
		printf ("\n");
#endif
	}
	return (cc == 'C' || cc == 'c');
}

void acknowledge (void)
{
#if _RUN_TIME < _rt_UNIXPCC
	/* Use this if kbhit/getch are available. */
	printf ("Press any key to continue: ");
	while (kbhit ()) getch ();
	while (!kbhit ());
	getch ();
	printf ("\n");
#elif __cplusplus
	/* Use this if kbhit/getch not available and C++ */
	char strbuf [MAX_INPUT];
	cout << "Press any key to continue: " << flush;
	cin.getline(strbuf, MAX_INPUT, '\n');
	cout << "\n" << flush;
#else
	/* Use this as last resort. Note, no flush!!! */
	printf ("Press any key to continue: ");
	(void)getc (stdin);
	printf ("\n");
#endif
	return;
}

void usage (void)
{
	extern char cs_OptchrC;

	(void)printf ("Usage: CS_COMP [%cc] [%cb] [%cd] [%ct] [source_dir [result_dir]]\n",cs_OptchrC,cs_OptchrC,cs_OptchrC,cs_OptchrC);
	return;
}
