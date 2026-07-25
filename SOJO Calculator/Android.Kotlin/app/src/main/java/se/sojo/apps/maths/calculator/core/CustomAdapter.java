package se.sojo.apps.maths.calculator.core;

import static se.sojo.apps.maths.calculator.core.Calculator.getTextSize;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;
import androidx.annotation.NonNull;
import java.util.ArrayList;
import se.sojo.apps.maths.calculator.R;

public class CustomAdapter extends ArrayAdapter<String> {
    public CustomAdapter(Context context, ArrayList<String> arrayList ) {
        super( context, R.layout.history_list_item, arrayList );
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, @NonNull ViewGroup parent ) {
        TextView textView;

        if( convertView == null ) {
            convertView = LayoutInflater.from(this.getContext()).inflate( R.layout.history_list_item, parent, false );
            textView = convertView.findViewById(R.id.tv_history_item);
            convertView.setTag(textView);
        }
        else {
            textView = (TextView) convertView.getTag();
        }

        textView.setText( getItem( position ));
        textView.setTextSize(getTextSize());

        return convertView;
    }
}
