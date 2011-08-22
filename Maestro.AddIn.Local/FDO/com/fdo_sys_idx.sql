/*----------------------------------------------------------------------*/
/* Name                                                                 */
/*  g_sys_i.sql - create indexes for standard system tables schema      */
/*                                                                      */
/* Description                                                          */
/*      Command file to create indexes for standard schema and system   */
/*      tables in a newly granted database.                             */
/*                                                                      */
/* Remarks                                                              */
/*      This file is executed by a call to adb_exec_file().  Portions   */
/*      of this script are enabled or disabled via #ifdef variables.    */
/*      In particular defining VISION_noschema disables the creation of */
/*      the so called schema table indexes.                             */
/*                                                                      */
/*----------------------------------------------------------------------*/

#ifndef VISION_noschema

create unique index f_schemainfo_idx on f_schemainfo(schemaname)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

create unique index f_dbopen_usrnum_idx on f_dbopen(usernum)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

#ifdef    FDO_FEATURE
#ifdef   FDO_VERSIONS
create unique index f_feature_pk_idx on f_feature(featid, ltid)
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5);

create index f_feature_class_idx on f_feature(featid, classid)
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5);


create index f_feature_lt_idx on f_feature(ltid, nextltid)
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5);


create unique index f_geometry_0_pk_idx on f_geometry_0(featid, scid, seq, ltid)
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5);

create index f_geometry_0_lt_idx on f_geometry_0(ltid, nextltid)
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5);
#endif
#endif

create unique index f_classdef_pk_idx on f_classdefinition(classid)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

#ifdef MySQL
#ifdef Char3Byte
create unique index f_classdef_schnm_idx on f_classdefinition(classname(150), schemaname(150))
#else
create unique index f_classdef_schnm_idx on f_classdefinition(classname, schemaname)
#endif
#else
create unique index f_classdef_schnm_idx on f_classdefinition(classname, schemaname)
#endif
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

create unique index f_attributedef_pk_idx on f_attributedefinition(tablename, classid, columnname)  
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

create unique index f_attributedep_pk_idx on f_attributedependencies(pktablename, fktablename)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;
             
create unique index f_association_pk_idx on f_associationdefinition(pseudocolname, pktablename, fktablename)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;
             
#ifdef MySQL
#ifdef Char1Byte
create unique index f_sad_pk_idx on f_sad(ownername, elementname, elementtype, name) 
#endif
#ifdef Char2Byte
create unique index f_sad_pk_idx on f_sad(ownername(150), elementname(150), elementtype, name(150)) 
#endif
#ifdef Char3Byte
create unique index f_sad_pk_idx on f_sad(ownername(100), elementname(100), elementtype, name(100)) 
#endif
#else
create unique index f_sad_pk_idx on f_sad(ownername, elementname, elementtype, name) 
#endif
#ifdef Oracle           
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

#ifdef MySQL
#ifdef Char1Byte
create unique index f_schemaoptions_pk_idx on f_schemaoptions(ownername, elementname, elementtype, name) 
#endif
#ifdef Char2Byte
create unique index f_schemaoptions_pk_idx on f_schemaoptions(ownername(150), elementname(150), elementtype, name(150)) 
#endif
#ifdef Char3Byte
create unique index f_schemaoptions_pk_idx on f_schemaoptions(ownername(100), elementname(100), elementtype, name(100)) 
#endif
#else
create unique index f_schemaoptions_pk_idx on f_schemaoptions(ownername, elementname, elementtype, name) 
#endif
#ifdef Oracle           
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

/* F_SEQUNCE table primary key */
#ifdef Oracle
create unique index f_sequence_seq_id on f_sequence(seqid)
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5);
#endif        
          
/*  Create the G_PLAN table indexes.                                */

/* Primary Key */
create unique index f_spatialcontext_scid_pk_idx on f_spatialcontext(scid)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

create unique index f_spatialcontext_name_pk_idx on f_spatialcontext(scname)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;


/*  Create the G_PLAN_GROUPS index              */
create unique index f_spatialcontextgroup_pk_idx on f_spatialcontextgroup(scgid)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

/*#endif VISION_sdo */

/* create the f_spatialcontextgeom index */
create unique index f_spatialcontextgeom_pk_idx on f_spatialcontextgeom(scid, geomtablename, geomcolumnname)
#ifdef Oracle
    pctfree 5
    storage (initial     4K
             next        4K
             minextents  1
             maxextents  9999
             pctincrease 5)
#endif
;

#endif

/* End */
