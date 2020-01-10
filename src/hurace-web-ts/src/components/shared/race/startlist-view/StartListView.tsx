import React from 'react';
import { StartListViewItem } from './StartListViewItem';
import { HeaderCard } from '../../../shared/HeaderCard';
import { StartList } from '../../../../models/StartList';
import {
    NoListEntryText,
    ColumnFlex
} from '../../../../theme/CustomComponents';

export const StartListView: React.FC<{ startList: StartList[] }> = ({
    startList
}) => {
    return (
        <ColumnFlex>
            <HeaderCard
                headerText="Startliste"
                contentStyles={
                    startList.length !== 0
                        ? { padding: 0, height: '100%', width: '100%' }
                        : {}
                }
            >
                {startList.length !== 0 ? (
                    <ColumnFlex>
                        {startList?.map(sl => (
                            <StartListViewItem
                                key={sl.startNumber}
                                startList={sl}
                            />
                        ))}
                    </ColumnFlex>
                ) : (
                    <NoListEntryText>Keine Startlisteneinträge</NoListEntryText>
                )}
            </HeaderCard>
        </ColumnFlex>
    );
};
