using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class BarrierDemo
{
    static void Main()
    {
        int pmCount = 1, baCount = 1, devCount = 4, qaCount = 2;
        var tombstones = new string[] { "需求分析", "代码编写", "功能稳定" };
        var milestone = new Barrier(pmCount, p => Console.WriteLine($"完成阶段：{p.CurrentPhaseNumber}-{tombstones[p.CurrentPhaseNumber]}，参与数：{p.ParticipantCount}，剩余{p.ParticipantsRemaining}"));

        milestone.AddParticipants(baCount);
        for (var i = 0; i < baCount; ++i) {
            ThreadPool.QueueUserWorkItem(state => {
                Console.WriteLine($"BA-{state}：完成需求分析的编写，当前阶段：{milestone.CurrentPhaseNumber}，参与数：{milestone.ParticipantCount}，剩余{milestone.ParticipantsRemaining}！");
                milestone.SignalAndWait();
            }, (i + 1));
        }
        milestone.SignalAndWait();
        milestone.RemoveParticipants(baCount);

        milestone.AddParticipants(devCount);
        for (var i = 0; i < devCount; ++i) {
            ThreadPool.QueueUserWorkItem(state => {
                Console.WriteLine($"Dev-{state}：完成代码编写，当前阶段：{milestone.CurrentPhaseNumber}，参与数：{milestone.ParticipantCount}，剩余{milestone.ParticipantsRemaining}！");
                milestone.SignalAndWait();
                Console.WriteLine($"Dev-{state}：完成测试，当前阶段：{milestone.CurrentPhaseNumber}，参与数：{milestone.ParticipantCount}，剩余{milestone.ParticipantsRemaining}！");
                milestone.SignalAndWait();
            }, (i + 1));
        }
        milestone.SignalAndWait();
        // 下面这行并不能保证在phase结束是运行
        // Console.WriteLine($"到达代码编写里程碑，当前阶段：{milestone.CurrentPhaseNumber}，参与数：{milestone.ParticipantCount}，剩余{milestone.ParticipantsRemaining}！");
        milestone.AddParticipants(qaCount);
        for (var i = 0; i < qaCount; ++i) {
            ThreadPool.QueueUserWorkItem(state => {
                Console.WriteLine($"Qa-{state}：完成测试，当前阶段：{milestone.CurrentPhaseNumber}，参与数：{milestone.ParticipantCount}，剩余{milestone.ParticipantsRemaining}！");
                milestone.SignalAndWait();
            }, (i + 1));
        }
        milestone.SignalAndWait();
    }
}