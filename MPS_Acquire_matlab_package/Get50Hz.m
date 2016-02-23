function [ A50 ] = Get50Hz(  t, sig  )
%GET50HZ 此处显示有关此函数的摘要
%   t - 输入信号的时间
%   sig - 输入信号

N=length(sig);
Y=fft(sig, N);
fs = N/(t(end) - t(1)); % frequency of sample

Af=abs(fft(sig, N)); % Amp of the frequency
ph = phase(Y); % Phase of the frequency
F=fs * ([1:N]-1)/N; % frequency axis
Af=Af/(N/2); % Amp of frequency

A50 = Af(F==50);
if isempty(A50)
    disp('No 50Hz signal found');
else
    disp(['50Hz amp: ' num2str(A50)]);
end
plot(F,Af);
% A50 = Af(F > 40 & F <55); % 很多时候只能获得60hz的信号。 @Todo: 在测量之前测试频谱，看看有没有我们要的信号
% numel(A50);
% % keyboard
% % find(F>48 & F<51)
% % A50 = A50(1);
% if numel(A50)<1
%     A50 = 0;
% else if numel(A50)>1
%     A50 = max(A50);
%     end
% end
% keyboard

end

