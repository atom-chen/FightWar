﻿using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class MeshFilterWrap
{
	public static LuaMethod[] regs = new LuaMethod[]
	{
		new LuaMethod("New", _CreateMeshFilter),
		new LuaMethod("GetClassType", GetClassType),
		new LuaMethod("__eq", Lua_Eq),
	};

	static LuaField[] fields = new LuaField[]
	{
		new LuaField("mesh", get_mesh, set_mesh),
		new LuaField("sharedMesh", get_sharedMesh, set_sharedMesh),
	};

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateMeshFilter(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			MeshFilter obj = new MeshFilter();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: MeshFilter.New");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, typeof(MeshFilter));
		return 1;
	}

	public static void Register(IntPtr L)
	{
		LuaScriptMgr.RegisterLib(L, "UnityEngine.MeshFilter", typeof(MeshFilter), regs, fields, typeof(UnityEngine.Component));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mesh(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MeshFilter obj = (MeshFilter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mesh");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mesh on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.mesh);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sharedMesh(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MeshFilter obj = (MeshFilter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sharedMesh");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sharedMesh on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.sharedMesh);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mesh(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MeshFilter obj = (MeshFilter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mesh");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mesh on a nil value");
			}
		}

		obj.mesh = LuaScriptMgr.GetUnityObject<Mesh>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sharedMesh(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		MeshFilter obj = (MeshFilter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name sharedMesh");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index sharedMesh on a nil value");
			}
		}

		obj.sharedMesh = LuaScriptMgr.GetUnityObject<Mesh>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetVarObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetVarObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

