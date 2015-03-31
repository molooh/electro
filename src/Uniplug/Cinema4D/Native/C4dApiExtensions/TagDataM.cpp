#include "TagDataM.h"
#include "c4d_tagdata.h"
#include "c4d_baselist.h"
#include "c4d_basedraw.h"

TagDataM::TagDataM(void)
{
}


TagDataM::~TagDataM(void)
{

}

Bool TagDataM::Message(GeListNode *node, Int32 type, void *data)
{
	switch (type)
	{
	case MSG_EDIT:

		break;
	case MSG_GETCUSTOMICON:
		break;
	case COLORSYSTEM_HSVTAB:
		break;
	case MSG_DOCUMENTINFO:
		{
			DocumentInfoData* did = (DocumentInfoData*)data;
			return MessageDocumentInfo(node, did);
		}
		break;
	case MSG_DESCRIPTION_GETINLINEOBJECT:
		break;
	case DRAW_PARAMETER_OGL_PRIMITIVERESTARTINDEX:
		break;
	}

	return true;
}

Bool TagDataM::MessageDocumentInfo(GeListNode *node, DocumentInfoData *data)
{
	// Do nothing. To be overridden (in C#)
	return true;
}

/*
BaseContainer *TagDataM::GetDataInstance(BaseObject *op)
{
	return op->GetDataInstance();
}

BaseContainer *TagDataM::GetDataInstance(GeListNode *node)
{
	BaseObject		*op   = (BaseObject*)node;
	BaseContainer *data = op->GetDataInstance();
	return data;
}
*/