function [ cb1m, fval, iter, exitflag, output ] = searchCurrent( m )
%专用函数，用于单独优化电流的大小。用在Simple Situation( Include
%Current)。此文件已经编译成p文件，如要应用改变，这需要重新编译pcode
%   此处显示详细说明

    I = m(4);
%     options.progTol = 1e-12;
    options.Method = 'csd';
%     options.FunEvals = 1000; % 默认1000
%     options.MaxIter = 600;
    options.Display = 'off';
%     options.numDiff = 1;
%     options.DerivativeCheck = 'on';
    global optimParam
    cb1m = m';
    MaxIter = 100;
    optTol = 1e-5;
    
    for iter = 1:MaxIter
        optimParam = [4 5 6];
        [cb1m,~,exitflag,output] = minFunc(@func_emf, [cb1m(1:3);I;cb1m(5:6)], options);
        optimParam = [];  
        [fval, grad] = func_emf(cb1m);
        if fval<optTol
            break;
        end
        I = I - fval / grad(4); % Newton Method
    end
    cb1m = [cb1m(1:3);I;cb1m(5:6)];

end

