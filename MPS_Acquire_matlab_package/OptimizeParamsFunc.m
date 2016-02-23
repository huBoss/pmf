function [ v ] = OptimizeParamsFunc( m )
%UNTITLED2 此处显示有关此函数的摘要
%   m = [ K, ke ];

global e e0 p i n k0 k
% ke = 1;%m(9);
ke = m(9);
K = m(1:8);
k = k0*ke;
for ii = 1:n
    e0(ii, 1:length(e(ii,:))) = e(ii,:)*K(ii);
end
% e0 = e.*K;;%dot(e,K);
exx = e0;
e0 = exx(:,1:31);
v = func_emf([p,i]');
e0 = exx(:,32:62);
v = v + func_emf([0,1,-0.3,100,0,0]');
% v = v / sqrt(dot(e0,e0)/n);


end

