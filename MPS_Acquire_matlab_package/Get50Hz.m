function [ A50 ] = Get50Hz(  t, sig  )
%GET50HZ �˴���ʾ�йش˺�����ժҪ
%   t - �����źŵ�ʱ��
%   sig - �����ź�

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
% A50 = Af(F > 40 & F <55); % �ܶ�ʱ��ֻ�ܻ��60hz���źš� @Todo: �ڲ���֮ǰ����Ƶ�ף�������û������Ҫ���ź�
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

