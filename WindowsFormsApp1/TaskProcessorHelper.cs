namespace Microshaoft
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
#if NETFRAMEWORK4_X
    using System.Windows.Forms;
#endif
    interface IDialogResultForm
    {
        void SetDialogResultProcess(params DialogResult[] results);
    }


    public static class TaskWaitingProcessorHelper
    {
#if NETFRAMEWORK4_X
        public static DialogResult ProcessWaitingShowDialog
                    (
                        IWin32Window ownerWindow
                        , Form dialogForm
                        , Action<Form> onProcessAction = null
                        , Func<Exception, Form, DialogResult> onCaughtExceptionProcessFunc = null
                    )
        {
            //var wait = new AutoResetEvent(false);
            DialogResult r = default(DialogResult);
            var IsCompleted = false;
            if (onProcessAction != null)
            {
                new Thread
                        (
                            new ThreadStart
                                (
                                    () =>
                                    {
                                        //wait.WaitOne();
                                        Thread.Sleep(10);
                                        try
                                        {
                                            
                                            onProcessAction(dialogForm);
                                            IsCompleted = true;
                                        }
                                        catch (Exception e)
                                        {
                                            //r = -1;
                                            if (onCaughtExceptionProcessFunc != null)
                                            {
                                                var rr = onCaughtExceptionProcessFunc(e, dialogForm);
                                                //IDialogResultForm form = dialogForm as IDialogResultForm;
                                                //form.SetDialogResultProcess(rr);

                                            }
                                        }
                                        finally
                                        {
                                            //TrySafeFormInvokeClose
                                            //    (
                                            //        dialogForm
                                            //        , onCaughtExceptionProcessFunc
                                            //    );
                                        }
                                    }
                                )
                        ).Start();
                //wait.Set();
                if (!IsCompleted)
                {
                    r = dialogForm.ShowDialog(ownerWindow);
                }
            }
            return r;
        }

        

        public static bool TrySafeFormInvokeAction
                                        (
                                            Form dialogForm
                                            , Action<Form> invokeAction
                                            , Func<Exception, DialogResult> onCaughtExceptionProcessFunc
                                        )
        {
            bool r = false;
            try
            {
                if
                (
                    dialogForm.IsHandleCreated
                    && !dialogForm.IsDisposed
                )
                {

                    dialogForm.Invoke
                            (
                                new Action
                                (
                                    () =>
                                    {
                                        invokeAction(dialogForm);
                                    }
                                )
                            

                                
                            );
                    Thread.Sleep(10);
                }
                r = true;
            }
            catch (Exception e)
            {
                r = false;
                if (onCaughtExceptionProcessFunc != null)
                {
                    onCaughtExceptionProcessFunc(e);
                }
            }
            return r;


        }


        private static bool TrySafeFormInvokeClose
                                (
                                    Form dialogForm
                                    , Func<Exception, DialogResult> onCaughtExceptionProcessFunc
                                )
        {
            var action = new Action<Form>
                        (
                            (x) =>
                            {
                                if
                                (
                                    dialogForm.IsHandleCreated
                                    && !dialogForm.IsDisposed
                                )
                                {
                                    dialogForm.Close();
                                }

                            }
                        );
            bool r = TrySafeFormInvokeAction
                (
                    dialogForm
                    , action
                    , onCaughtExceptionProcessFunc
                );
               
            return r;
        }
#endif

    }
}
