#pragma once
#include "c4d_tagdata.h"
#include "c4d_baselist.h"

// We need this construction because SWIG is unable to handle the void pointer/casting mechanism
// used in the Message method. 

class MessageParams
{
public:

};

class TagDataM :
	public TagData
{
public:
	TagDataM(void);
	virtual ~TagDataM(void);

	virtual Bool Message(GeListNode *node, Int32 type, void *data);
	virtual Bool MessageDocumentInfo(GeListNode *node, DocumentInfoData *data);
	// virtual Bool GetDDescription(GeListNode *node, DDescriptionParams *descparams);

	/*
	static BaseContainer *GetDataInstance(BaseObject *op);
	static BaseContainer *GetDataInstance(GeListNode *node);
	*/
};
