//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.8
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace C4d {

public class BaseSelectData : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal BaseSelectData(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(BaseSelectData obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~BaseSelectData() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          C4dApiPINVOKE.delete_BaseSelectData(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public int a {
    set {
      C4dApiPINVOKE.BaseSelectData_a_set(swigCPtr, value);
    } 
    get {
      int ret = C4dApiPINVOKE.BaseSelectData_a_get(swigCPtr);
      return ret;
    } 
  }

  public int b {
    set {
      C4dApiPINVOKE.BaseSelectData_b_set(swigCPtr, value);
    } 
    get {
      int ret = C4dApiPINVOKE.BaseSelectData_b_get(swigCPtr);
      return ret;
    } 
  }

  public BaseSelectData() : this(C4dApiPINVOKE.new_BaseSelectData(), true) {
  }

}

}
