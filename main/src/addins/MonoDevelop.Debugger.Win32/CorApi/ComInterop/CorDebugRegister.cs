﻿using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugRegister
        {
            // registers (potentially) available on all architectures
            // Note that these overlap with the architecture-specific
            // registers
            //
            // NOTE: On IA64, REGISTER_FRAME_POINTER represents the BSP register.

            REGISTER_INSTRUCTION_POINTER = 0,
            REGISTER_STACK_POINTER,
            REGISTER_FRAME_POINTER,


            // X86 registers

            REGISTER_X86_EIP = 0,
            REGISTER_X86_ESP,
            REGISTER_X86_EBP,

            REGISTER_X86_EAX,
            REGISTER_X86_ECX,
            REGISTER_X86_EDX,
            REGISTER_X86_EBX,

            REGISTER_X86_ESI,
            REGISTER_X86_EDI,

            REGISTER_X86_FPSTACK_0,
            REGISTER_X86_FPSTACK_1,
            REGISTER_X86_FPSTACK_2,
            REGISTER_X86_FPSTACK_3,
            REGISTER_X86_FPSTACK_4,
            REGISTER_X86_FPSTACK_5,
            REGISTER_X86_FPSTACK_6,
            REGISTER_X86_FPSTACK_7,


            // AMD64 registers

            REGISTER_AMD64_RIP = 0,
            REGISTER_AMD64_RSP,
            REGISTER_AMD64_RBP,

            REGISTER_AMD64_RAX,
            REGISTER_AMD64_RCX,
            REGISTER_AMD64_RDX,
            REGISTER_AMD64_RBX,

            REGISTER_AMD64_RSI,
            REGISTER_AMD64_RDI,

            REGISTER_AMD64_R8,
            REGISTER_AMD64_R9,
            REGISTER_AMD64_R10,
            REGISTER_AMD64_R11,
            REGISTER_AMD64_R12,
            REGISTER_AMD64_R13,
            REGISTER_AMD64_R14,
            REGISTER_AMD64_R15,

            // Xmm FP

            REGISTER_AMD64_XMM0,
            REGISTER_AMD64_XMM1,
            REGISTER_AMD64_XMM2,
            REGISTER_AMD64_XMM3,
            REGISTER_AMD64_XMM4,
            REGISTER_AMD64_XMM5,
            REGISTER_AMD64_XMM6,
            REGISTER_AMD64_XMM7,
            REGISTER_AMD64_XMM8,
            REGISTER_AMD64_XMM9,
            REGISTER_AMD64_XMM10,
            REGISTER_AMD64_XMM11,
            REGISTER_AMD64_XMM12,
            REGISTER_AMD64_XMM13,
            REGISTER_AMD64_XMM14,
            REGISTER_AMD64_XMM15,


            // IA64 registers

            REGISTER_IA64_BSP = REGISTER_FRAME_POINTER,

            // To get a particular general register, add the register number
            // to REGISTER_IA64_R0.  The same also goes for floating point
            // registers.
            //
            // For example, if you need REGISTER_IA64_R83,
            // use REGISTER_IA64_R0 + 83.
            REGISTER_IA64_R0  = REGISTER_IA64_BSP + 1,
            REGISTER_IA64_F0  = REGISTER_IA64_R0  + 128,


            // ARM registers (@ARMTODO: FP?)

            REGISTER_ARM_PC = 0,
            REGISTER_ARM_SP,
            REGISTER_ARM_R0,
            REGISTER_ARM_R1,
            REGISTER_ARM_R2,
            REGISTER_ARM_R3,
            REGISTER_ARM_R4,
            REGISTER_ARM_R5,
            REGISTER_ARM_R6,
            REGISTER_ARM_R7,
            REGISTER_ARM_R8,
            REGISTER_ARM_R9,
            REGISTER_ARM_R10,
            REGISTER_ARM_R11,
            REGISTER_ARM_R12,
            REGISTER_ARM_LR,

            // ARM64 registers

            REGISTER_ARM64_PC = 0,
            REGISTER_ARM64_SP,
            REGISTER_ARM64_FP,
            REGISTER_ARM64_X0,
            REGISTER_ARM64_X1,
            REGISTER_ARM64_X2,
            REGISTER_ARM64_X3,
            REGISTER_ARM64_X4,
            REGISTER_ARM64_X5,
            REGISTER_ARM64_X6,
            REGISTER_ARM64_X7,
            REGISTER_ARM64_X8,
            REGISTER_ARM64_X9,
            REGISTER_ARM64_X10,
            REGISTER_ARM64_X11,
            REGISTER_ARM64_X12,
            REGISTER_ARM64_X13,
            REGISTER_ARM64_X14,
            REGISTER_ARM64_X15,
            REGISTER_ARM64_X16,
            REGISTER_ARM64_X17,
            REGISTER_ARM64_X18,
            REGISTER_ARM64_X19,
            REGISTER_ARM64_X20,
            REGISTER_ARM64_X21,
            REGISTER_ARM64_X22,
            REGISTER_ARM64_X23,
            REGISTER_ARM64_X24,
            REGISTER_ARM64_X25,
            REGISTER_ARM64_X26,
            REGISTER_ARM64_X27,
            REGISTER_ARM64_X28,
            REGISTER_ARM64_LR,

            // other architectures here

        } CorDebugRegister;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugRegister
    {
        REGISTER_AMD64_RIP = 0,
        REGISTER_ARM64_PC = 0,
        REGISTER_ARM_PC = 0,
        REGISTER_INSTRUCTION_POINTER = 0,
        REGISTER_X86_EIP = 0,
        REGISTER_AMD64_RSP = 1,
        REGISTER_ARM64_SP = 1,
        REGISTER_ARM_SP = 1,
        REGISTER_STACK_POINTER = 1,
        REGISTER_X86_ESP = 1,
        REGISTER_AMD64_RBP = 2,
        REGISTER_ARM64_FP = 2,
        REGISTER_ARM_R0 = 2,
        REGISTER_FRAME_POINTER = 2,
        REGISTER_IA64_BSP = 2,
        REGISTER_X86_EBP = 2,
        REGISTER_AMD64_RAX = 3,
        REGISTER_ARM64_X0 = 3,
        REGISTER_ARM_R1 = 3,
        REGISTER_IA64_R0 = 3,
        REGISTER_X86_EAX = 3,
        REGISTER_AMD64_RCX = 4,
        REGISTER_ARM64_X1 = 4,
        REGISTER_ARM_R2 = 4,
        REGISTER_X86_ECX = 4,
        REGISTER_AMD64_RDX = 5,
        REGISTER_ARM64_X2 = 5,
        REGISTER_ARM_R3 = 5,
        REGISTER_X86_EDX = 5,
        REGISTER_AMD64_RBX = 6,
        REGISTER_ARM64_X3 = 6,
        REGISTER_ARM_R4 = 6,
        REGISTER_X86_EBX = 6,
        REGISTER_AMD64_RSI = 7,
        REGISTER_ARM64_X4 = 7,
        REGISTER_ARM_R5 = 7,
        REGISTER_X86_ESI = 7,
        REGISTER_AMD64_RDI = 8,
        REGISTER_ARM64_X5 = 8,
        REGISTER_ARM_R6 = 8,
        REGISTER_X86_EDI = 8,
        REGISTER_AMD64_R8 = 9,
        REGISTER_ARM64_X6 = 9,
        REGISTER_ARM_R7 = 9,
        REGISTER_X86_FPSTACK_0 = 9,
        REGISTER_AMD64_R9 = 10,
        REGISTER_ARM64_X7 = 10,
        REGISTER_ARM_R8 = 10,
        REGISTER_X86_FPSTACK_1 = 10,
        REGISTER_AMD64_R10 = 11,
        REGISTER_ARM64_X8 = 11,
        REGISTER_ARM_R9 = 11,
        REGISTER_X86_FPSTACK_2 = 11,
        REGISTER_AMD64_R11 = 12,
        REGISTER_ARM64_X9 = 12,
        REGISTER_ARM_R10 = 12,
        REGISTER_X86_FPSTACK_3 = 12,
        REGISTER_AMD64_R12 = 13,
        REGISTER_ARM64_X10 = 13,
        REGISTER_ARM_R11 = 13,
        REGISTER_X86_FPSTACK_4 = 13,
        REGISTER_AMD64_R13 = 14,
        REGISTER_ARM64_X11 = 14,
        REGISTER_ARM_R12 = 14,
        REGISTER_X86_FPSTACK_5 = 14,
        REGISTER_AMD64_R14 = 15,
        REGISTER_ARM64_X12 = 15,
        REGISTER_ARM_LR = 15,
        REGISTER_X86_FPSTACK_6 = 15,
        REGISTER_AMD64_R15 = 16,
        REGISTER_ARM64_X13 = 16,
        REGISTER_X86_FPSTACK_7 = 16,
        REGISTER_AMD64_XMM0 = 17,
        REGISTER_ARM64_X14 = 17,
        REGISTER_AMD64_XMM1 = 18,
        REGISTER_ARM64_X15 = 18,
        REGISTER_AMD64_XMM2 = 19,
        REGISTER_ARM64_X16 = 19,
        REGISTER_AMD64_XMM3 = 20,
        REGISTER_ARM64_X17 = 20,
        REGISTER_AMD64_XMM4 = 21,
        REGISTER_ARM64_X18 = 21,
        REGISTER_AMD64_XMM5 = 22,
        REGISTER_ARM64_X19 = 22,
        REGISTER_AMD64_XMM6 = 23,
        REGISTER_ARM64_X20 = 23,
        REGISTER_AMD64_XMM7 = 24,
        REGISTER_ARM64_X21 = 24,
        REGISTER_AMD64_XMM8 = 25,
        REGISTER_ARM64_X22 = 25,
        REGISTER_AMD64_XMM9 = 26,
        REGISTER_ARM64_X23 = 26,
        REGISTER_AMD64_XMM10 = 27,
        REGISTER_ARM64_X24 = 27,
        REGISTER_AMD64_XMM11 = 28,
        REGISTER_ARM64_X25 = 28,
        REGISTER_AMD64_XMM12 = 29,
        REGISTER_ARM64_X26 = 29,
        REGISTER_AMD64_XMM13 = 30,
        REGISTER_ARM64_X27 = 30,
        REGISTER_AMD64_XMM14 = 31,
        REGISTER_ARM64_X28 = 31,
        REGISTER_AMD64_XMM15 = 32,
        REGISTER_ARM64_LR = 32,
        REGISTER_IA64_F0 = 131,
    }
}